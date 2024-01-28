using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace WebSalesMvcWithAngular.Controllers
{
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public  TokensController(IAntiforgery antiforgery)
        {
           _antiforgery = antiforgery;
        }
        
        [HttpGet]
        [Route("antiforgery-token")]
        public IActionResult GetAntiforgeryToken()
        {
            var antiforgeryToken = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(antiforgeryToken.RequestToken);
        }
    }
}
