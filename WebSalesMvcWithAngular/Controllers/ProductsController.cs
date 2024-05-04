﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ApiController]
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
            catch (NotFoundException)
            {
                return NotFound("Nenhuma categoria encontrada.");
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
        [Route("post-product")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Produto não fornecido" +
                        ",.");
                }

                var department = await _departmentService.FindByIdAsync((int)product.DepartmentId);

                var category = await _categoryService.FindByIdAsync((int)product.CategoryId);

                var newProduct = new Product
                {
                    Name = product.Name,
                    Price = (double)product.Price,
                    Category = category,
                    Department = department,
                    CategoryId = (int)product.CategoryId,
                    DepartmentId = (int)product.DepartmentId,
                    InventoryUnitMeas = (WebSalesMvcWithAngular.Models.Enums.InventoryUnitMeas)product.InventoryUnitMeas,
                    ImageUrl = product.ImageUrl,
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
                    return NotFound("Produto não encontrada.");
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
                        productToUpdate.InventoryCost = productDto.InventoryCost ?? productToUpdate.InventoryCost;
                        productToUpdate.InventoryQuantity = productToUpdate.InventoryQuantity + productDto.InventoryQuantity ?? productToUpdate.InventoryQuantity;
                        productToUpdate.AcquisitionCost = productDto.AcquisitionCost ?? productToUpdate.AcquisitionCost;
                        productToUpdate.MinimumInventoryQuantity = productDto.MinimumInventoryQuantity ?? productToUpdate.MinimumInventoryQuantity;

                        await _productService.UpdateAsync(productToUpdate);

                        productDto.Margin = productToUpdate.CalculateMargin();
                        productDto.Profit = productToUpdate.CalculateProfit();
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
                Profit = p.CalculateProfit(),
                Margin = p.CalculateMargin(),
                CMV = p.CalculateCMV()

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
