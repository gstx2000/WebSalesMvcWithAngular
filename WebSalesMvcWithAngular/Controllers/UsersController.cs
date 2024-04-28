using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using WebSalesMvcWithAngular.DTOs.Requests;
using WebSalesMvcWithAngular.Services.Interfaces;

[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly PasswordRecoveryService _passwordRecoveryService;
    private readonly IIdentityService _identityService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        PasswordRecoveryService passwordRecoveryService,
        IIdentityService identityService,
        ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _passwordRecoveryService = passwordRecoveryService;
        _identityService = identityService;
        _logger = logger;   
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
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [Route("password-recovery")]
    public async Task<IActionResult> PasswordRecovery([FromBody] PasswordRecoveryRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);
                var frontEndHost = "localhost:44493";
                var resetLink = $"{Request.Scheme}://{frontEndHost}/redefine-password?email={user.Email}&token={encodedToken}";
                var encodedResetLink = HtmlEncoder.Default.Encode(resetLink);

                await _passwordRecoveryService.SendEmailAsync(request.Email, "Recuperação de senha", "Equipe de suporte", "gst203002@gmail.com", $"Clique no link para redefinir sua senha: {encodedResetLink}");

                return Ok();
            }
            else
            {
                return NotFound("Nenhum usuário encontrado com este e-mail.");
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("reset-password")]

    public async Task<IActionResult> ResetPassword([FromBody] RegisterRequest request)
    {
        _logger.LogInformation($"----------------------------------------EMAIL RECEBIDO: {request.Email}");
        _logger.LogInformation($"----------------------------------------TOKEN RECEBIDO: {request.RecoveryToken}");
        _logger.LogInformation($"----------------------------------------request RECEBIDO: {request}");
      
        if (!ModelState.IsValid)
        {
            // Handle validation errors
            return BadRequest(ModelState);
        }

        byte[] decodedBytes = WebEncoders.Base64UrlDecode(request.RecoveryToken);
        string token = Encoding.UTF8.GetString(decodedBytes);

        var user = await _userManager.FindByEmailAsync(request.Email);


        if (user != null)
        {
            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
        }
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Usuário ou token inválido.");
        }

        if (user == null)
        {
            return BadRequest("Usuário não encontrado.");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(ModelState);
    }
}
