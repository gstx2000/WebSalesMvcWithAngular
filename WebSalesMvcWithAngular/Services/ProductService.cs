using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

public class ProductService
{
    private readonly WebSalesMvcContext _context;

    public ProductService(WebSalesMvcContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> FindAllAsync()
    {
        return await _context.Product
                        .Include(x => x.Category)
                        .Include(x => x.Department)
                        .ToListAsync();
    }

    public async Task<List<Product>> FindAllPaginatedAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new ArgumentException("Page number and page size must be greater than 0.");

        return await _context.Product
                              .Include(x => x.Category)
                              .Include(x => x.Department)
                              .OrderBy(x => x.Id)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
    }

    //This is used for counting the number of records for the paginator in FRONTEND.
    public async Task<int> CountAllAsync()
    {
        return await _context.Product.CountAsync();
    }

    public async Task<Product> FindByIdAsync(int id)
    {
        return await _context.Product
            .Include(p => p.Department)
            .Include(p => p.Category)  
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    public async Task InsertAsync(Product product)
    {
        _context.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Product.FindAsync(id);
        _context.Product.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> FindByNameAsync(string productName, int? categoryId)
    {
        var query = _context.Product
            .Where(p => EF.Functions.Like(p.Name, $"%{productName}%"));

        query = query.Include(p => p.Department) 
                     .Include(p => p.Category) 
                     .AsQueryable(); 

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        return await query.ToListAsync();
    }

}
