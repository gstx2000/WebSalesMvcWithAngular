using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

namespace WebSalesMvc.Services
{
    public class DepartmentService
    {
        private readonly WebSalesMvcContext _context;

        public DepartmentService(WebSalesMvcContext context)
        {
            _context = context;
        }
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department.FindAsync(id);
        }

        public async Task AddSellerToDepartmentAsync(int departmentId, Seller seller)
        {
            var department = await _context.Department.FindAsync(departmentId);

            if (department != null)
            {
                department.AddSeller(seller);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Department.Update(department);
            await _context.SaveChangesAsync();
        }
    }
}
