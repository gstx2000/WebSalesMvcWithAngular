using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Controllers.SuppliersController.Responses;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> FindAllAsync();
        Task<(IEnumerable<IndexSupplierResponse>, int productCount)> FindAllPaginatedAsync(int pageNumber = 1, int pageSize = 10);
        Task<int> CountAllAsync();
        Task<Supplier> FindByIdAsync(int supplierId);
        Task InsertAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
        Task<List<Supplier>> FindByNameAsync(string supplierName);
        Task<List<Product>> GetProducts(int supplierId);

        Task<int> GetProductCount(int supplierId);
    }
}