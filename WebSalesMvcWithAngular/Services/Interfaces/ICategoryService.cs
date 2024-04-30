using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> FindAllAsync();
        Task InsertAsync(Category category);
        Task<Category> FindByIdAsync(int id);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task <Category> GetCategoryByNameAsync(string categoryName);
    }
}