using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;

namespace WebSalesMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebSalesMvcContext _context;
        private readonly DepartmentService _departmentService;
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;


        public ProductsController(WebSalesMvcContext context, DepartmentService departmentService, CategoryService categoryService, ProductService productService)
        {
            _context = context;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var viewModel = await _productService.FindAllAsync();

            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.FindAllAsync();

            var viewModel = new ProductFormViewModel {  Categories = categories };
            return View(viewModel);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();
                var viewModel = new ProductFormViewModel { Product = product, Categories = categories };
                return View(viewModel);
            }

            var category = await _categoryService.FindbyIdAsync(product.CategoryId);
            if (category == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Categoria inválida." });
            }

            product.Category = category;
            product.Department = category?.Department;

            await _productService.InsertAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, Product product)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = await _productService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado." });
            }

            List<Category> categories = await _categoryService.FindAllAsync();

            var viewModel = new ProductFormViewModel
            {
                Product = obj,
                Categories = categories
            };

            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id incompatível." });
            }

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();
                var viewModel = new ProductFormViewModel { Product = product, Categories = categories };
                return View(viewModel);
            }
            
            try
            {
                Category category = await _categoryService.FindbyIdAsync(product.CategoryId);
                product.Department = category?.Department;
                await _productService.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            var products = await _productService.FindByNameAsync(searchTerm);

            return PartialView("_ProductSearchResults", products);
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
