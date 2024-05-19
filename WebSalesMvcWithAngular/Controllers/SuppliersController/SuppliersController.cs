using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Controllers.SuppliersController.Responses;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvcWithAngular.Controllers.SuppliersController
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly ISupplierService _supplierService;
        public SuppliersController(WebSalesMvcContext context, ISupplierService supplierService)
        {
            _context = context;
            _supplierService = supplierService;
        }

        [HttpGet]
        [Route("get-suppliers")]

        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            try
            {
                var supplier = await _supplierService.FindAllAsync();

                if (supplier == null)
                {
                    return NotFound();
                }
                return Ok(supplier);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet]
        [Route("get-suppliers-paginated")]
        
        public async Task<ActionResult<IEnumerable<IndexSupplierResponse>>> GetSuppliersPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (supplierDtos, totalItems) = await _supplierService.FindAllPaginatedAsync(page, pageSize);
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                
               
                var pagedResult = new PagedResult<IndexSupplierResponse>
                {
                    items = supplierDtos,
                    page = page,
                    pageSize = pageSize,
                    totalItems = totalItems,
                    totalPages = totalPages
                };

                if (!pagedResult.items.Any())
                {
                    return NoContent();
                }

                return Ok(pagedResult);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet("{id}")]
        [Route("get-supplier/{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _supplierService.FindByIdAsync(id);

                if (supplier == null)
                {
                    return NotFound();
                }
                return Ok(supplier);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpPatch("{id}")]
        [ValidateAntiForgeryToken]
        [Route("edit-supplier/{id}")]

        public async Task<IActionResult> PatchSupplier(int? id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            try
            {
                await _supplierService.UpdateAsync(supplier);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("post-supplier")]
        public async Task<IActionResult> PostSupplier([FromBody] Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _supplierService.InsertAsync(supplier);
                    return CreatedAtAction("Details", new { id = supplier.SupplierId }, supplier);
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpDelete("{id}")]
        [Route("delete-supplier")]

        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierService.FindByIdAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }
            try
            {
                await _supplierService.DeleteAsync(id);
                return Ok();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        public async Task<ActionResult<List<Product>>> GetProducts(int supplierId)
        {
            try
            {
                var products = await _supplierService.GetProducts(supplierId);
                if (products == null || products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(products);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }
        private bool SupplierExists(int? id)
        {
            return (_context.Supplier?.Any(e => e.SupplierId == id)).GetValueOrDefault();
        }
    }
}
