using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> FindAllAsync();
        Task<List<Supplier>> FindAllPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAllAsync();
        Task<Supplier> FindByIdAsync(int supplierId);
        Task InsertAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
        Task<List<Supplier>> FindByNameAsync(string supplierName);
        Task<List<Product>> GetProducts(int supplierId);
    }
}