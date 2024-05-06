
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IAdressService
    {
        Task<List<Adress>> FindAllAsync();
        Task<List<Adress>> FindAllPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAllAsync();
        Task<Adress> FindByIdAsync(int AdressId);
        Task InsertAsync(Adress adress);
        Task UpdateAsync(Adress adress);
        Task DeleteAsync(int id);
        Task<List<Adress>> FindByStreetNameAsync(string StreetName);
    }
}
