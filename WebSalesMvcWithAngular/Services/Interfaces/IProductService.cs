using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> FindAllAsync();
        Task<List<Product>> FindAllPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAllAsync();
        Task<Product> FindByIdAsync(int id);
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> FindByNameAsync(string productName, int? categoryId = null);
        Task<List<Supplier>> GetSuppliers(int productId);
        Task AddSupplierAsync(Product product, Supplier supplier, double supplyPrice);
    }
}