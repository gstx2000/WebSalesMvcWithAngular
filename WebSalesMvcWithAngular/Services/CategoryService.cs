using Microsoft.EntityFrameworkCore;

using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvc.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly WebSalesMvcContext _context;

        public CategoryService(WebSalesMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllAsync()
        {
            return await _context.Category
                .Include(s => s.Department)
                .Include(s => s.Products) 
                .ToListAsync();
        }

        public async Task<List<CategoryDTO>> FindAllDTOAsync()
        {
            var categories = await _context.Category
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Products)
                .ToListAsync();

            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                DepartmentName = c.Department?.Name,
                ProductCount = GetCategoryProductCount(c),
                IsSubCategory = c.IsSubCategory
            }).ToList();

            return categoryDTOs;
        }

        private int GetCategoryProductCount(Category category)
        {
            int count = category.Products.Count;
            count += GetSubCategoryProductCount(category);
            return count;
        }

        private int GetSubCategoryProductCount(Category category)
        {
            int count = 0;
            if (category.SubCategories != null)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    if (subCategory.Products != null)
                    {
                        count += subCategory.Products.Count;
                    }
                    count += GetSubCategoryProductCount(subCategory);                }
            }
            return count;
        }
        public async Task InsertAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            return await _context.Category
                .Include(obj => obj.Department)
                .Include(obj => obj.Products)
                .FirstOrDefaultAsync(obj => obj.Id == id);
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
        public async Task DeleteAsync(int id)
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
        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            var category = await _context.Category
                .FirstOrDefaultAsync(p => EF.Functions.Like(p.Name, $"%{categoryName}%"));

            return category;
        }
    }
}

        