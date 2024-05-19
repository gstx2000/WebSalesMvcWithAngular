using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Controllers.ProductsController.Requests;
using WebSalesMvcWithAngular.Controllers.ProductsController.Responses;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> FindAllAsync();
        Task<(IEnumerable<IndexProductResponse>, int productCount)> FindAllPaginatedAsync(int pageNumber = 1, int pageSize = 10);
        Task<int> CountAllAsync();
        Task<Product> FindByIdAsync(int id);
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> FindByNameAsync(string productName, int? categoryId = null);
        Task<List<Supplier>> GetSuppliers(int productId);
        Task AddSupplierAsync(Product product, Supplier supplier, decimal supplyPrice);

        Task UpdateInventoryAsync(Product product, decimal receiptQuantity, int? supplierId = null);
    }
}