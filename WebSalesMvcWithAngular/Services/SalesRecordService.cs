using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Controllers;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

namespace WebSalesMvc.Services
{
    public class SalesRecordService
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
            _context.SalesRecord.Remove(sale);
            await _context.SaveChangesAsync();
        }

    }
}
