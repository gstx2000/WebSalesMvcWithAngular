using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSalesMvcWithAngular.DTOs.Requests;
using WebSalesMvcWithAngular.Services;
using WebSalesMvcWithAngular.Services.Interfaces;

[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly PasswordRecoveryService _passwordRecoveryService;
    private readonly IIdentityService _identityService;

    public UsersController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        PasswordRecoveryService passwordRecoveryService,
        IIdentityService identityService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _passwordRecoveryService = passwordRecoveryService;
        _identityService = identityService;
    }



[HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [Route("register")]

    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {

        if (ModelState.IsValid)
        {
                var result = await _identityService.RegisterUser(registerRequest);

                if (result.Success)
                {
                    return Ok();
                }
                else if (result.Errors.Count > 0)
                {
                return BadRequest(result);
                }
       
        }
        return StatusCode(StatusCodes.Status500InternalServerError);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [Route("login")]

    public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
    {
        if (ModelState.IsValid)
        {
            var result = await _identityService.LoginUser(loginRequest);

            if (result.Success)
            {
                Response.Headers.Add("Authorization", "Bearer " + result.Token);
                return Ok(result);
            }
            else if (result.Errors.Count > 0)
            {
                return BadRequest(result);
            }
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok();
    }

    //[HttpPost]
    //[AllowAnonymous]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> PasswordRecovery(PasswordRecoveryViewModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var user = await _userManager.FindByEmailAsync(model.Email);

    //        if (user != null)
    //        {
    //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //            var resetLink = _urlHelper.Action("ResetPassword", "Users", new { email = user.Email, token = token }, Request.Scheme);
    //            var encodedResetLink = HtmlEncoder.Default.Encode(resetLink);

    //            await _passwordRecoveryService.SendEmailAsync(model.Email, "Recuperação de senha", "Equipe de suporte", "gst203002@gmail.com", $"Clique no link para redefinir sua senha: {encodedResetLink}");

    //            return RedirectToAction("PasswordRecoveryConfirmation");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Email inválido.");
    //    return Ok();
    //}

    //[HttpGet]
    //[AllowAnonymous]
    //public async Task<IActionResult> ResetPassword(string email, string token)
    //{
    //    var user = await _userManager.FindByEmailAsync(email);
    //    var isTokenValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

    //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
    //    {
    //        return RedirectToAction("PasswordResetError", new { error = "Usuário ou token inválido." });
    //    }

    //    if (user == null)
    //        return RedirectToAction("PasswordResetError", new { error = "Usuário não encontrado." });

    //    if (!isTokenValid)
    //    {
    //        return RedirectToAction("PasswordResetError", new { error = "Token inválido." });
    //    }

    //    return Ok();
    
}
