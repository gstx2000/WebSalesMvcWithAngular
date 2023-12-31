using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<Product> FindByIdAsync(int id)
    {
        return await _context.Product.Include(p => p.Department).FirstOrDefaultAsync(m => m.Id == id);
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

    public async Task<List<Product>> FindByNameAsync(string productName)
    {
        return await _context.Product
            .Where(p => EF.Functions.Like(p.Name, $"%{productName}%"))
            .ToListAsync();
    }

  
}
