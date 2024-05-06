using WebSalesMvcWithAngular.Services.Interfaces;
using WebSalesMvcWithAngular.Models;
using WebSalesMvc.Data;
using Microsoft.EntityFrameworkCore;

namespace WebSalesMvcWithAngular.Services
{
    public class AdressesService : IAdressService
    {

        private readonly WebSalesMvcContext _context;

        public AdressesService(WebSalesMvcContext context)
        {
            _context = context;
        }
        public async Task<List<Adress>> FindAllAsync()
        {
            return await _context.Adress
                        .ToListAsync();
        }
        public async Task<List<Adress>> FindAllPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than 0.");

            return await _context.Adress
                                  .OrderBy(x => x.AdressId)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }
        public async Task<int> CountAllAsync()
        {
            return await _context.Adress.CountAsync();
        }
        public async Task<Adress> FindByIdAsync(int adressId)
        {
            return await _context.Adress
           .FirstOrDefaultAsync(m => m.AdressId == adressId);
        }
        public async Task InsertAsync(Adress adress)
        {
            _context.Add(adress);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Adress adress)
        {
            _context.Update(adress);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var adress = await _context.Adress.FindAsync(id);
            _context.Adress.Remove(adress);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Adress>> FindByStreetNameAsync(string streetName)
        {
            var query = _context.Adress
                .Where(p => EF.Functions.Like(p.Logradouro, $"%{streetName}%"));

            return await query.ToListAsync();
        }
    }
}
