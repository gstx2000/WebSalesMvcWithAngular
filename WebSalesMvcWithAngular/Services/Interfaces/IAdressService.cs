
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IAdressService
    {
        Task<List<Address>> FindAllAsync();
        Task<List<Address>> FindAllPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAllAsync();
        Task<Address> FindByIdAsync(int AdressId);
        Task InsertAsync(Address adress);
        Task UpdateAsync(Address adress);
        Task DeleteAsync(int id);
        Task<List<Address>> FindByStreetNameAsync(string StreetName);
    }
}
