using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Controllers;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.Controllers.ProductsController.Responses;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvcWithAngular.Controllers.ProductsController
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SalesRecordsController> _logger;
        private readonly IMapper _mapper;
        public ProductsController(WebSalesMvcContext context, IDepartmentService departmentService,
            ILogger<SalesRecordsController> logger, ICategoryService categoryService, IProductService productService,
            ISupplierService supplierService, IMapper mapper)
        {
            _context = context;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
            _supplierService = supplierService;
            _mapper = mapper;
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
        [Route("get-products-paginated")]
        public async Task<ActionResult<PagedResult<IndexProductResponse>>> GetProductsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (productDtos, totalItems) = await _productService.FindAllPaginatedAsync(page, pageSize);
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var pagedResult = new PagedResult<IndexProductResponse>
                {
                    items = productDtos,
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

            catch (NotFoundException)
            {
                return NotFound("Nenhum produto encontrada.");
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
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação. ");
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("post-product")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO product, [FromForm] IFormFile? imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = $"/images/{imageFile.FileName}";
                }

                if (product == null)
                {
                    return BadRequest("Produto não fornecido pela requisição.");
                }

                var department = await _departmentService.FindByIdAsync((int)product.DepartmentId);
                var category = await _categoryService.FindByIdAsync((int)product.CategoryId);

                var newProduct = new Product
                {
                    Name = product.Name,
                    Price = (decimal)product.Price,
                    Category = category,
                    Department = department,
                    CategoryId = (int)product.CategoryId,
                    DepartmentId = (int)product.DepartmentId,
                    InventoryUnitMeas = (Models.Enums.InventoryUnitMeas)product.InventoryUnitMeas,
                    ImageUrl = product.ImageUrl,
                    SubCategoryId = product.SubCategoryId
                };


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
                    await _productService.InsertAsync(newProduct);
                    return CreatedAtAction("Details", new { id = product.Id }, product);
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
                return StatusCode(500, "Erro interno da aplicação");
            }
        }

        [HttpPatch("{id}")]
        [Route("edit-product/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] ProductDTO productDto)
        {
            try
            {
                var productToUpdate = await _productService.FindByIdAsync(id);

                if (productToUpdate == null)
                {
                    return NotFound("Produto não encontrado.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        productToUpdate.Name = productDto.Name ?? productToUpdate.Name;
                        productToUpdate.Price = productDto.Price ?? productToUpdate.Price;
                        productToUpdate.DepartmentId = productDto.DepartmentId ?? productToUpdate.DepartmentId;
                        productToUpdate.CategoryId = productDto.CategoryId ?? productToUpdate.CategoryId;
                        productToUpdate.ImageUrl = productDto.ImageUrl ?? productToUpdate.ImageUrl;
                        productToUpdate.InventoryUnitMeas = productDto.InventoryUnitMeas ?? productToUpdate.InventoryUnitMeas;
                        productToUpdate.SubCategoryId = productDto.SubCategoryId ?? productToUpdate.SubCategoryId;


                        if (productDto.CategoryId.HasValue)
                        {
                            var category = await _categoryService.FindByIdAsync(productDto.CategoryId.Value);
                            if (category != null)
                            {
                                productToUpdate.Category = category;
                            }
                            else
                            {
                                return BadRequest("Categoria não encontrada.");
                            }
                        }
                        if (productDto.DepartmentId.HasValue)
                        {
                            var department = await _departmentService.FindByIdAsync(productDto.DepartmentId.Value);
                            if (department != null)
                            {
                                productToUpdate.Department = department;
                            }
                            else
                            {
                                return BadRequest("Departamento não encontrado.");
                            }
                        }
                        await _productService.UpdateAsync(productToUpdate);
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(productToUpdate.Id))
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
        [Route("edit-inventory/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInventory(int id, [FromBody] ProductDTO productDto)
        {
            try
            {
                var productToUpdate = await _productService.FindByIdAsync(id);

                if (productToUpdate == null)
                {
                    return NotFound("Produto não encontrada.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (productDto.SupplierId != null)
                        {
                            var supplier = await _supplierService.FindByIdAsync((int)productDto.SupplierId);
                            if (supplier != null)
                            {
                                await _productService.AddSupplierAsync(productToUpdate, supplier, productDto.AcquisitionCost ?? 0);
                            }
                        }

                        productToUpdate.InventoryCost = productDto.InventoryCost ?? productToUpdate.InventoryCost;
                        productToUpdate.InventoryQuantity += productDto.InventoryQuantity ?? 0;
                        productToUpdate.AcquisitionCost = productDto.AcquisitionCost ?? productToUpdate.AcquisitionCost;
                        productToUpdate.MinimumInventoryQuantity = productDto.MinimumInventoryQuantity ?? productToUpdate.MinimumInventoryQuantity;

                        var receiptQuantity = productDto.InventoryQuantity ?? 0;
                        await _productService.UpdateInventoryAsync(productToUpdate, receiptQuantity, (int)productDto.SupplierId);

                        productDto.CMV = productToUpdate.CalculateCMV();

                        return Ok(productDto);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(productToUpdate.Id))
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
            catch (UnauthorizedException)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet("get-product-by-name/{productName}/{categoryId?}")]
        public async Task<IActionResult> GetProductByName(string productName, int? categoryId = null)
        {
            var products = await _productService.FindByNameAsync(productName, categoryId);

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            var productDtos = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                DepartmentName = p.Department.Name,
                CategoryId = p.CategoryId,
                DepartmentId = p.DepartmentId,
                InventoryUnitMeas = p.InventoryUnitMeas,
                AcquisitionCost = p.AcquisitionCost,
                InventoryQuantity = p.InventoryQuantity,
                InventoryCost = p.InventoryCost,
                MinimumInventoryQuantity = p.MinimumInventoryQuantity,
                TotalInventoryValue = p.TotalInventoryValue,
                TotalInventoryCost = p.TotalInventoryCost,
                ImageUrl = p.ImageUrl,
                CMV = p.CalculateCMV(),
                Suppliers = p.Suppliers.Select(ps => new SupplierDTO
                {
                    SupplierId = ps.SupplierId,
                    Name = ps.Supplier.Name,
                    SupplyPrice = ps.SupplyPrice,
                }).ToList()
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("get-full-product-by-name/{productName}/{categoryId?}")]
        public async Task<IActionResult> GetFullProductByName(string productName, int? categoryId = null)
        {
            var products = await _productService.FindByNameAsync(productName, categoryId);

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
