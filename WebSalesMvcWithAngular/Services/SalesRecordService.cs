using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

namespace WebSalesMvc.Services
{
    public class SalesRecordService
    {
        private readonly WebSalesMvcContext _context;

        public SalesRecordService(WebSalesMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindAllAsync()
        {
            return await _context.SalesRecord
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
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

        public async Task InsertAsync(SalesRecord salesRecord)
        {
            _context.SalesRecord.Add(salesRecord);
            await _context.SaveChangesAsync();
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
