using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> FindAllAsync();
        Task<Department> FindByIdAsync(int id);
        Task AddSellerToDepartmentAsync(int departmentId, Seller seller);
        Task InsertAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
        bool DepartmentExistsAsync(int id);
    }
}