using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services.Exceptions;
using Microsoft.Extensions.Logging;


namespace WebSalesMvc.Controllers
{
    [Route("api/[controller]")]
    public class SalesRecordsController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly ProductService _productService;
        private readonly ILogger<SalesRecordsController> _logger;


        public SalesRecordsController(WebSalesMvcContext context, SalesRecordService salesRecordService, SellerService sellerService, ProductService productService, ILogger<SalesRecordsController> logger)
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
            var list = await _salesRecordService.FindAllAsync();
            try
            {
                var sales = await _salesRecordService.FindAllAsync();
                if (sales == null || sales.Count == 0)
                {
                    return NoContent();
                }

                return Ok(sales);
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

        [HttpPost]
        [Route("post-salesrecord")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] SalesRecord salesRecord)
        {
            try
            {
                if (salesRecord == null || salesRecord.SoldProducts == null || salesRecord.SoldProducts.Count == 0)
                {
                    return BadRequest("Venda não fornecida ou produtos não especificados.");
                }

                salesRecord.SellerId = 1;
                salesRecord.Id = 0;
                salesRecord.Date = DateTime.Now;

                if (ModelState.IsValid)
                {
                    foreach (var soldProduct in salesRecord.SoldProducts)
                    {
                        soldProduct.Id = 0;
                        soldProduct.SalesRecordId = salesRecord.Id;

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
