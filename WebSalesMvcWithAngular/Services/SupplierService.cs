using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvcWithAngular.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly WebSalesMvcContext _context;
        private readonly ILogger<SupplierService> _logger;
        public SupplierService(WebSalesMvcContext context, ILogger<SupplierService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Supplier>> FindAllAsync()
        {
            return await _context.Supplier
                        .ToListAsync();
        }
        public async Task<List<Supplier>> FindAllPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than 0.");

            return await _context.Supplier
                                  .OrderBy(x => x.SupplierId)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }
        public async Task<int> CountAllAsync()
        {
            return await _context.Supplier.CountAsync();
        }
        public async Task<Supplier> FindByIdAsync(int supplierId)
        {
            return await _context.Supplier
           .FirstOrDefaultAsync(m => m.SupplierId == supplierId);
        }
        public async Task InsertAsync(Supplier supplier)
        {
            _logger.LogInformation("Attempting to insert supplier: {Supplier}", supplier);
            _context.Supplier.Add(supplier);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Inserted supplier: {Supplier}", supplier);
        }
        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Update(supplier);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProducts(int supplierId)
        {
            return await _context.ProductSupplier
               .Where(ps => ps.SupplierId == supplierId)
               .Select(ps => ps.Product)
               .ToListAsync();
        }
        public async Task<List<Supplier>> FindByNameAsync(string supplierName)
        {
            var query = _context.Supplier
                .Where(p => EF.Functions.Like(p.Name, $"%{supplierName}%"));

            return await query.ToListAsync();
        }
    }
}
