using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Interfaces;
using WebSalesMvcWithAngular.Controllers.SuppliersController.Responses;
using AutoMapper;

namespace WebSalesMvcWithAngular.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly WebSalesMvcContext _context;
        private readonly ILogger<SupplierService> _logger;
        private readonly IMapper _mapper;

        public SupplierService(WebSalesMvcContext context, ILogger<SupplierService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<Supplier>> FindAllAsync()
        {
            return await _context.Supplier
                        .ToListAsync();
        }
        public async Task<(IEnumerable<IndexSupplierResponse>, int productCount)> FindAllPaginatedAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than 0.");

            var supplierCount = await CountAllAsync();
            var productResponses = await _context.Supplier
           .OrderBy(p => p.SupplierId)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ProjectTo<IndexSupplierResponse>(_mapper.ConfigurationProvider)
           .ToListAsync();
            
            foreach (var productResponse in productResponses) {
                productResponse.ProductCount = await GetProductCount((int)productResponse.SupplierId);
            }
            return (productResponses, supplierCount);
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
            _context.Supplier.Add(supplier);
            await _context.SaveChangesAsync();
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

        public async Task<int> GetProductCount(int supplierId)
        {
            return await _context.ProductSupplier
               .Where(ps => ps.SupplierId == supplierId)
               .Select(ps => ps.ProductId) 
               .CountAsync();
        }

        public async Task<List<Supplier>> FindByNameAsync(string supplierName)
        {
            var query = _context.Supplier
                .Where(p => EF.Functions.Like(p.Name, $"%{supplierName}%"));

            return await query.ToListAsync();
        }
    }
}
