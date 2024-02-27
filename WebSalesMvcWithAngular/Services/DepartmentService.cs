using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Department.OrderBy(x => x.Name)
                //.Include(d => d.Sellers)
                .ToListAsync();
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
        public async Task InsertAsync(Department department)
        {
            _context.Department.Add(department);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Department department)
        {
            _context.Department.Update(department);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department != null)
            {
                _context.Department.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
        public bool DepartmentExistsAsync(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }

    }
}
