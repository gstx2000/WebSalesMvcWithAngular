using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebSalesMvc.Controllers;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvc.Services
{
    public class SalesRecordService : ISalesRecordService
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
        public async Task<(decimal Sum, int Count, int PendingSales)> GetWeekReportAsync()
        {
            DateTime date = DateTime.Now;
            GregorianCalendar calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime startDay;
            DateTime endDay;

            int weekOfTheMonth = (int)Math.Ceiling((date - firstDayOfMonth).TotalDays / 7);
            switch (weekOfTheMonth)
            {
                case 1:
                    startDay = firstDayOfMonth;
                    endDay = firstDayOfMonth.AddDays(6);
                    break;

                case 2:
                    startDay = firstDayOfMonth.AddDays(7);
                    endDay = firstDayOfMonth.AddDays(15);
                    break;

                case 3:
                    startDay = firstDayOfMonth.AddDays(16);
                    endDay = firstDayOfMonth.AddDays(23);
                    break;

                case 4:
                    startDay = firstDayOfMonth.AddDays(24);
                    endDay = firstDayOfMonth.AddDays(31);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(weekOfTheMonth), weekOfTheMonth, null);
            }
            _logger.LogInformation($"Semana definida: {weekOfTheMonth}");
            var sumWeekSales = await _context.SalesRecord
                              .Where(p => p.Date >= startDay && p.Date <= endDay)
                              .Where(p => p.Status == Models.Enums.SaleStatus.Faturada)
                              .SumAsync(p => p.Amount);

            var countWeekSales = await _context.SalesRecord
                            .Where(p => p.Date >= startDay && p.Date <= endDay)
                            .Where(p => p.Status == Models.Enums.SaleStatus.Faturada)
                            .CountAsync();

            var countPendingtSales = await _context.SalesRecord
                         .Where(p => p.Status == Models.Enums.SaleStatus.Pendente)
                         .CountAsync();
            return (sumWeekSales, countWeekSales, countPendingtSales);
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

            return salesRecord.Id;
        }

        public async Task<(string, decimal?)> RemoveStockQuantity(int productId, decimal quantityToRemove)
        {
            var (inventoryQuantity, acquisitionCosts, inventoryReceipts) = await GetTotalStockQuantity(productId, quantityToRemove);

            if (inventoryQuantity < quantityToRemove)
            {
                return ("Sem capacidade suficiente no estoque para realizar a venda.", 0);
            }
            else
            {
                decimal? accumulatedAcquisitionCost = acquisitionCosts.Sum();

                foreach (var lot in inventoryReceipts)
                {
                    decimal amountToRemoveFromLot = Math.Min((byte)quantityToRemove, (byte)lot.InventoryQuantity);
                    lot.InventoryQuantity -= amountToRemoveFromLot;
                    quantityToRemove -= amountToRemoveFromLot;

                    if (quantityToRemove <= 0) break;
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return (null, accumulatedAcquisitionCost);
                }
                catch (Exception ex)
                {
                    return ("Erro ao manipular o banco de dados.", null);
                }
            }
        }
        public async Task<(decimal?, List<decimal?>, List<InventoryReceipt>)> GetTotalStockQuantity(int productId, decimal? quantity)
        {
            List<decimal?> supplyPrices = new List<decimal?>();
            decimal? runningTotal = 0;

            var inventoryReceipts = await _context.InventoryReceipt
              .Where(ir => ir.ProductId == productId && ir.InventoryQuantity > 0)
              .ToListAsync();

            foreach (var receipt in inventoryReceipts)
            {
                decimal? amountToTake = Math.Min((byte)quantity.Value, (byte)receipt.InventoryQuantity);

                runningTotal += amountToTake;
                quantity -= amountToTake;
                supplyPrices.Add(receipt.SupplyPrice * amountToTake);

                if (runningTotal >= quantity.Value) break;
            }

            return (runningTotal, supplyPrices.TakeWhile((_, index) => index < runningTotal).ToList(), inventoryReceipts);
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
