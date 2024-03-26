using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvc.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<SalesRecordsController> _logger;

        public ProductsController(WebSalesMvcContext context, IDepartmentService departmentService, ILogger<SalesRecordsController> logger, ICategoryService categoryService, IProductService productService)
        {
            _context = context;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.FindAllAsync();
                if (products == null || products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(products);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Nenhuma categoria encontrada.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet]
        [Route("get-products-paginated")]
        public async Task<ActionResult<PagedResult<Product>>> GetProductsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var products = await _productService.FindAllPaginatedAsync(page, pageSize);
                var totalItems = await _productService.CountAllAsync(); 
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var productDtos = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    DepartmentName = p.Department.Name
                }).ToList();

                var result = new PagedResult<ProductDTO>
                {
                    items = productDtos,
                    page = page,
                    pageSize = pageSize,
                    totalItems = totalItems,
                    totalPages = totalPages
                };

                if (result.items.Count == 0)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Nenhum produto encontrada.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet("{id}")]
        [Route("get-product/{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID não fornecido.");
                }

                var product = await _productService.FindByIdAsync(id.Value);

                if (product == null)
                {
                    return NotFound($"Produto com o ID {id} não foi encontrado.");
                }

                return Ok(product);

            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação. ");
            }
        }

        [HttpPost]
        [Route("post-product")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Produto não fornecido" +
                        ",.");
                }

                var department = await _departmentService.FindByIdAsync(product.DepartmentId);

                var category = await _categoryService.FindByIdAsync(product.CategoryId);


                if (department == null)
                {
                    return BadRequest("Departamento não encontrado.");
                }

                if (category == null)
                {
                    return BadRequest("Categoria não encontrada.");
                }

                product.Category = category;

                product.Department = department;


                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Erro produto: ");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            _logger.LogError(error.ErrorMessage);
                        }
                    }

                    return UnprocessableEntity(ModelState);
                }
                if (ModelState.IsValid)
                {
                    await _productService.InsertAsync(product);
                    return CreatedAtAction("Details", new { id = product.Id }, product);
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação");
            }
        }

        [HttpPut("{id}")]
        [Route("edit-product/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("ID fornecido difere do ID que vai ser atualizado.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _productService.UpdateAsync(product);

                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.Id))
                        {
                            return NotFound("Produto não encontrada.");
                        }
                        else
                        {
                            return StatusCode(500, "Erro de simultaneidade.");
                        }
                    }
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpDelete("{id}")]
        [Route("delete-product/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Produto não encontrada.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet("get-product/{productName}/{categoryId?}")]
        public async Task<IActionResult> GetProductByName(string productName, int? categoryId)
        {
            var products = await _productService.FindByNameAsync(productName, categoryId);
            
            var productDtos = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                DepartmentName = p.Department.Name,
                CategoryId = p.CategoryId,
                DepartmentId = p.DepartmentId
            }).ToList();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(productDtos);
        }

        private bool ProductExists(int id)
        {
                return _context.Product.Any(e => e.Id == id);
        }
    }
}
