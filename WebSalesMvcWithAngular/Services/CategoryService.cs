using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;

namespace WebSalesMvc.Services
{
    public class CategoryService
    {
        private readonly WebSalesMvcContext _context;

        public CategoryService(WebSalesMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllAsync()
        {
            return await _context.Category.Include(s => s.Department).ToListAsync();
        }
        public async Task InsertAsync(Category obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }
        public async Task<Category> FindbyIdAsync(int id)
        {
            return await _context.Category.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task UpdateAsync(Category obj)
        {
            bool hasAny = await _context.Category.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrado.");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Category.FindAsync(id);
                _context.Category.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }
        public async Task AddProductToCategoryAsync(int categoryId, Product product)
        {
            var category = await _context.Category.FindAsync(categoryId);
            if (category != null)
            {
                product.Category = category;
                category.AddProduct(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
        