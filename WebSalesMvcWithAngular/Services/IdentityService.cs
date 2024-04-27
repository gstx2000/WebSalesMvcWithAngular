using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebSalesMvcWithAngular.Configurations;
using WebSalesMvcWithAngular.DTOs.Requests;
using WebSalesMvcWithAngular.DTOs.Responses;
using WebSalesMvcWithAngular.Services.Interfaces;

namespace WebSalesMvcWithAngular.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<User> signInManager,
                               UserManager<User> userManager,
                               IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }
        public async Task<RegisterUserResponse> RegisterUser(RegisterRequest request)
        {
            var registerUserResponse = new RegisterUserResponse(false);

            var newUSer = new User
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUSer, request.Password);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(newUSer, false);
            }
            else
            { //This for is made for replace the default Identity errors messsages for a desired message.
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateUserName":
                        case "DuplicateEmail":
                                registerUserResponse.AddError("O email já está em uso.");
                        break;
                        case "InvalidEmail":
                        case "InvalidUserName":
                                registerUserResponse.AddError("O endereço de e-mail fornecido é inválido.");
                        break;
                        case "PasswordTooShort":
                            registerUserResponse.AddError("A senha deve ter no mínimo 6 dígitos.");
                        break;
                        case "PasswordRequiresNonAlphanumeric":
                            registerUserResponse.AddError("A senha deve conter um caractere especial.");
                        break;
                        case "PasswordRequiresLower":
                            registerUserResponse.AddError("A senha deve conter letras minúsculas.");
                        break;
                        case "PasswordRequiresUpper":
                            registerUserResponse.AddError("A senha deve conter letras maiúsculas.");
                        break;
                        case "PasswordMismatch":
                            registerUserResponse.AddError("A senhas estão diferentes.");
                        break;
                        case "InvalidPassword":
                            registerUserResponse.AddError("Senha inválida.");
                        break;
                        case "PasswordRequiresDigit":
                            registerUserResponse.AddError("A senha deve conter números.");
                        break;
                        default:
                            registerUserResponse.AddError(error.Description); // Fallback to the default error description
                        break;
                    }
                }
            }
            return registerUserResponse;
        }
        public async Task<LoginUserResponse> LoginUser(LoginRequest loginRequest)
        {
            //*This function is supposed to return a LoginUserResponse always
            //The GenerateJWTToken returns it authenticated with the JWT.
            var loginUserResponse = new LoginUserResponse(false);

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);

            if (result.Succeeded)
            {
                var jwtResponse = await GenerateJWTToken(loginRequest.Email);
                return jwtResponse;
            }
            if (!result.Succeeded) {
                if (result.IsLockedOut)
                {
                    loginUserResponse.AddError("Essa conta está bloqueada. Entre em contato com o suporte.");
                }
                else if (result.IsNotAllowed)
                {
                    loginUserResponse.AddError("Você não tem permissão.");
                }
                else if (result.RequiresTwoFactor)
                {
                    loginUserResponse.AddError("É necessário fazer a confirmação de duas etapas.");
                }
                else
                {
                    loginUserResponse.AddError("Usuário ou senha incorretos.");
                }
            }
            return loginUserResponse;   
        }
        public async Task<LoginUserResponse> GenerateJWTToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var tokenClaims = await GetClaims(user);

            var expireDate = DateTime.Now.AddHours(_jwtOptions.ExpirationDate);

            var jwt = new JwtSecurityToken(

                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: tokenClaims,   
                notBefore: DateTime.Now,
                expires: expireDate,
                signingCredentials: _jwtOptions.SigninCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new LoginUserResponse(
                success: true,
                token: token,
                expireDate: expireDate
                );

        }
        public async Task<List<Claim>> GetClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync((User)user);
            var roles = await _userManager.GetRolesAsync((User)user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));

            }
            return (List<Claim>)claims;
        }
    }
}

    

