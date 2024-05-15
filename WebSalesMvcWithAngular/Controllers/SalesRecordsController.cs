using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Models.Reports;
using WebSalesMvcWithAngular.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvc.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SalesRecordsController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly ISalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly IProductService _productService;
        private readonly ILogger<SalesRecordsController> _logger;
        public SalesRecordsController(WebSalesMvcContext context,
            ISalesRecordService salesRecordService, SellerService sellerService, IProductService productService,
            ILogger<SalesRecordsController> logger)
        {
            _context = context;
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-salesrecords")]
        public async Task<ActionResult<List<SalesRecord>>> GetSalesRecords()
        {
            try
            {
                var sales = await _salesRecordService.FindAllAsync();
                if (sales == null || sales.Count == 0)
                {
                    return NoContent();
                }

                return Ok(sales);
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

        [HttpGet("{id}")]
        [Route("get-salesrecord/{id}")]
        public async Task<ActionResult> GetSaleById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID não fornecido.");
                }

                var sale = await _salesRecordService.FindByIdAsync(id.Value);

                if (sale == null)
                {
                    return NotFound($"Produto com o ID {id} não foi encontrado.");
                }

                return Ok(sale);

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

        [HttpGet]
        [Route("get-salesrecords-to-invoice")]
        public async Task<ActionResult<PagedResult<SalesRecord>>> GetSalesRecordToInvoice([FromQuery] int page = 1,
         [FromQuery] int pageSize = 10)
        {
            try
            {
                var (salesRecords, totalItems) = await _salesRecordService.FindAlltoInvoiceAsync(page, pageSize);

                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var salesRecordDtos = salesRecords.Select(p => new SalesRecordDTO
                {
                    Id = p.Id,
                    Status = p.Status,
                    Amount = p.Amount,
                    Date = p.Date
                }).ToList();

                var result = new PagedResult<SalesRecordDTO>
                {
                    items = salesRecordDtos,
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
        [Route("get-salesrecords-paginated")]
        public async Task<ActionResult<PagedResult<SalesRecord>>> GetSalesRecordPaginated([FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {
                var salesRecords = await _salesRecordService.FindAllPaginatedAsync(page, pageSize);
                var totalItems = await _salesRecordService.CountAllAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var salesRecordDtos = salesRecords.Select(p => new SalesRecordDTO
                {
                    Id = p.Id,
                    Status = p.Status,
                    Amount = p.Amount,
                    Date = p.Date
                }).ToList();

                var result = new PagedResult<SalesRecordDTO>
                {
                    items = salesRecordDtos,
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

        [HttpPost]
        [Route("post-salesrecord")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] SalesRecord salesRecord)
        {
            List<decimal?> acquisitionCosts = new List<decimal?>();
            
            try
            {
                if (salesRecord == null || salesRecord.SoldProducts == null || salesRecord.SoldProducts.Count == 0)
                {
                    return BadRequest("Venda não fornecida ou produtos não especificados.");
                }
                try
                {
                    foreach (var soldProduct in salesRecord.SoldProducts)
                    {
                        var (message, acquisitionCost) = await _salesRecordService.RemoveStockQuantity(soldProduct.ProductId, soldProduct.Quantity);
                        if (!string.IsNullOrEmpty(message))
                        {
                            return BadRequest(message);
                        }
                        acquisitionCosts.Add(acquisitionCost);
                    }
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                decimal? accumulatedAcquisitionCost = acquisitionCosts.Sum();
                var totalRevenue = salesRecord.SoldProducts.Sum(soldProduct => (soldProduct.Price * soldProduct.Quantity));
                salesRecord.Profit = totalRevenue - accumulatedAcquisitionCost ?? 0;
                salesRecord.SellerId = 1;
                salesRecord.Id = 0;
                salesRecord.Date = DateTime.Now;

                if (ModelState.IsValid)
                {
                    foreach (var soldProduct in salesRecord.SoldProducts)
                    {
                        soldProduct.Id = 0;
                        soldProduct.SalesRecordId = salesRecord.Id;
                        soldProduct.Margin = soldProduct.CalculateMargin();
                        _context.SoldProducts.Add(soldProduct);
                    }
                    await _salesRecordService.InsertAsync(salesRecord);

                    return CreatedAtAction("Details", new { id = salesRecord.Id }, salesRecord);
                }
                else
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new { x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });

                    foreach (var error in errors)
                    {
                        foreach (var errorMessage in error.Errors)
                        {
                            _logger.LogError($"ModelState error for {error.Key}: {errorMessage}");
                        }
                    }

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



        [HttpPut("{id}")]
        [Route("edit-salesrecord/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] SalesRecord sale)
        {
            try
            {
                if (id != sale.Id)
                {
                    return BadRequest("ID fornecido difere do ID que vai ser atualizado.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _salesRecordService.UpdateAsync(sale);

                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SalesRecordExists(sale.Id))
                        {
                            return NotFound("Venda não encontrada.");
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
        [Route("delete-salesrecord/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _salesRecordService.DeleteAsync(id);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Venda não encontrada.");
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
        [Route("simple-search")]
        public async Task<ActionResult<List<SalesRecord>>> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            return Ok(new
            {
                MinDate = minDate.Value.ToString("yyyy-MM-dd"),
                MaxDate = maxDate.Value.ToString("yyyy-MM-dd"),
                Data = result
            });
        }

        [HttpGet]
        [Route("get-week-report")]
        public async Task<IActionResult> GetWeekReport()
        {

            try
            {
                var weekTotal = await _salesRecordService.GetWeekReportAsync();

                var sales = new SalesData
                {
                    Sum = weekTotal.Sum != 0 ? weekTotal.Sum : 0,
                    Count = weekTotal.Count != 0 ? weekTotal.Count : 0,
                    PendingSales = weekTotal.PendingSales != 0 ? weekTotal.PendingSales : 0
                };

                return Ok(sales);
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
        [Route("grouping-search")]
        public async Task<ActionResult<List<SalesRecord>>> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);

            return Ok(new
            {
                MinDate = minDate.Value.ToString("yyyy-MM-dd"),
                MaxDate = maxDate.Value.ToString("yyyy-MM-dd"),
                Data = result
            });
        }
        private bool SalesRecordExists(int id)
        {
            return _context.SalesRecord.Any(e => e.Id == id);
        }
    }
}
