using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Controllers;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvc.Services
{
    public class SalesRecordService: ISalesRecordService
    {
        private readonly WebSalesMvcContext _context;
        private readonly ILogger<SalesRecordsController> _logger;

        public SalesRecordService(WebSalesMvcContext context, ILogger<SalesRecordsController> logger)
        {
            _context = context;
            _logger = logger;   
        }
        public async Task<List<SalesRecord>> FindAllAsync()
        {
            return await _context.SalesRecord
                //.Include(x => x.Seller)
                //.Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
        public async Task<List<SalesRecord>> FindAllPaginatedAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Número de página deve ser maior que 0");
            return await _context.SalesRecord
                                  .Include(x => x.SoldProducts)
                                  .OrderByDescending(x => x.Date)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }
        public async Task<int> CountAllAsync()
        {
            return await _context.SalesRecord.CountAsync();
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<(List<SalesRecord> Results, int TotalCount)> FindAlltoInvoiceAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Número de página deve ser maior que 0");

            int totalCount = await _context.SalesRecord
                                           .Where(x => x.Status == 0)
                                           .CountAsync();

            var results = await _context.SalesRecord
                                        .Include(x => x.SoldProducts)
                                        .Where(x => x.Status == 0)
                                        .OrderByDescending(x => x.Date)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
            return (results, totalCount);
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }

        public async Task<int> InsertAsync(SalesRecord salesRecord)
        {
            _context.SalesRecord.Add(salesRecord);
            _context.ChangeTracker.Entries().ToList().ForEach(e => Console.WriteLine($"Entity: {e.Entity.GetType().Name}, State: {e.State}"));

            await _context.SaveChangesAsync();
                _logger.LogInformation($"SalesRecord inserted. ID: {salesRecord.Id}, Date: {salesRecord.Date}, Amount: {salesRecord.Amount}");

                _logger.LogInformation("Transaction committed successfully.");
            return salesRecord.Id;

        }
        public async Task UpdateAsync(SalesRecord sale)
        {
            _context.SalesRecord.Update(sale);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var sale = await _context.SalesRecord.FindAsync(id);
            var soldProducts = await _context.SoldProducts
            .Where(p => p.SalesRecordId == sale.Id)
            .ToListAsync();
           
            _context.SalesRecord.Remove(sale);
            _context.SoldProducts.RemoveRange(soldProducts);

            await _context.SaveChangesAsync();
        }

        public async Task<SalesRecord> FindByIdAsync(int? id)
        {
            return await _context.SalesRecord
                                  .Include(m => m.SoldProducts)
                                  .ThenInclude(sp => sp.Product)
                                  .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
