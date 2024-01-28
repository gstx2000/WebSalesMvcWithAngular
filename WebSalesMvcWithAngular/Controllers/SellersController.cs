using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;

namespace WebSalesMvc.Controllers 
{    
    [Route("api/[controller]")]
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments =  await _departmentService.FindAllAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                return View();
            }

            var department = await _departmentService.FindByIdAsync(seller.DepartmentId);

            if (department == null)
            {
            }

            seller.Department = department;

            await _sellerService.InsertAsync(seller);

            department.AddSeller(seller);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
            }

            var obj = await _sellerService.FindbyIdAsync(id.Value);

            if (obj == null)
            {
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            
          
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
            }

            var obj = await _sellerService.FindbyIdAsync(id.Value);

            if (obj == null)
            {
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
            }

            var obj = await _sellerService.FindbyIdAsync(id.Value);

            if (obj == null)
            {
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                return View();
            }

            if (id != seller.Id)
            {
            }
         
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
    
        }
        
    }
}