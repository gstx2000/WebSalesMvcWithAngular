using WebSalesMvc.Models;
using WebSalesMvcWithAngular.DTOs;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> FindAllAsync();
        Task<List<CategoryDTO>> FindAllDTOAsync();
        Task InsertAsync(Category category);
        Task<Category> FindByIdAsync(int id);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task <Category> GetCategoryByNameAsync(string categoryName);
    }
}