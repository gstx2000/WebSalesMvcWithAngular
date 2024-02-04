using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Exceptions;

namespace WebSalesMvc.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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

        [HttpGet("get-product/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            var products = await _productService.FindByNameAsync(productName);

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        private bool ProductExists(int id)
        {
                return _context.Product.Any(e => e.Id == id);
        }
    }
}
