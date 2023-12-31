using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using static WebSalesMvc.Models.SalesRecord;

namespace WebSalesMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly WebSalesMvcContext _context;
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly ProductService _productService;

        public SalesRecordsController(WebSalesMvcContext context, SalesRecordService salesRecordService, SellerService sellerService, ProductService productService)
        {
            _context = context;
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _productService = productService;   
        }
        public async Task<IActionResult> Index()
        {
            var list = await _salesRecordService.FindAllAsync();

            if (list.Count == 0)
            {
                ViewBag.Message = "Sem registro de venda.";
                return View();
            }
            foreach (var record in list)
            {
                record.Seller = await _sellerService.FindbyIdAsync(record.SellerId);
            }

            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }
        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();
            var salesRecord = new SalesRecord();
            var products = await _productService.FindAllAsync();

            var viewModel = new SalesRecordsCreateViewModel
            {
                SalesRecord = salesRecord,
                Sellers = sellers,
                Products = products
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecordsCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var salesRecordview = viewModel.SalesRecord;
                var productsToRemove = new List<Product>();

                if (salesRecordview != null)
                {
                    var selectedProducts = JsonConvert.DeserializeObject<List<SelectedProduct>>(viewModel.SelectedProductsJson);

                    salesRecordview.Products = salesRecordview.Products ?? new List<Product>();

                    foreach (var product in salesRecordview.Products)
                    {
                        if (!selectedProducts.Any(sp => sp.Id == product.Id))
                        {
                            productsToRemove.Add(product);
                        }
                    }

                    foreach (var productToRemove in productsToRemove)
                    {
                        salesRecordview.Products.Remove(productToRemove);
                    }

                    foreach (var selectedProduct in selectedProducts)
                    {
                        var existingProduct = await _productService.FindByIdAsync(selectedProduct.Id);

                        if (existingProduct != null)
                        {
                            salesRecordview.Products.Add(existingProduct);
                        }
                    }



                    salesRecordview.UpdateAmount();



                    await _salesRecordService.InsertAsync(salesRecordview);

                    return RedirectToAction(nameof(Index));
                }
            }

            var sellers = await _sellerService.FindAllAsync();
            viewModel.Sellers = sellers;

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord.FindAsync(id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            var viewModel = new SalesRecordsCreateViewModel
            {
                SalesRecord = salesRecord,
                Sellers = await _sellerService.FindAllAsync() 
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecordsCreateViewModel viewModel)
        {
            if (id != viewModel.SalesRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.SalesRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesRecordExists(viewModel.SalesRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.Sellers = await _sellerService.FindAllAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesRecord = await _context.SalesRecord.FindAsync(id);
            _context.SalesRecord.Remove(salesRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesRecordExists(int id)
        {
            return _context.SalesRecord.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }

    }
}
