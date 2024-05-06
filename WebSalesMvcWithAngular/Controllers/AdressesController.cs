using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Services;
using WebSalesMvcWithAngular.Services.Exceptions;

namespace WebSalesMvcWithAngular.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly AdressesService _adressSrevice;
        public AdressesController(WebSalesMvcContext context, AdressesService adressSrevice)
        {
            _context = context;
            _adressSrevice = adressSrevice; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Adress>>> GetAdresses()
        {
            try
            {
                var adress = await _adressSrevice.FindAllAsync();

                if (adress == null)
                {
                    return NotFound();
                }
                return Ok(adress);
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
        public async Task<ActionResult<Adress>> GetAdress(int id)
        {
            try
            {
                var adress = await _adressSrevice.FindByIdAsync(id);

                if (adress == null)
                {
                    return NotFound();
                }
                return Ok(adress);
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PatchAdress(int? id, Adress adress)
        {
            if (id != adress.AdressId)
            {
                return BadRequest();
            }

            try
            {
              await _adressSrevice.UpdateAsync(adress);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdressExists(id))
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
        public async Task<ActionResult<Adress>> PostAdress(Adress adress)
        {
            try
            {
                if (ModelState.IsValid) {
                    await _adressSrevice.InsertAsync(adress);
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

            return CreatedAtAction("GetAdress", new { id = adress.AdressId }, adress);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdress(int id)
        {
            var adress = await _adressSrevice.FindByIdAsync(id);

            if (adress == null)
            {
                return NotFound();
            }
            try
            {
                await _adressSrevice.DeleteAsync(id);
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
        private bool AdressExists(int? id)
        {
            return (_context.Adress?.Any(e => e.AdressId == id)).GetValueOrDefault();
        }
    }
}
