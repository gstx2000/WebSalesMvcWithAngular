using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebSalesMvcWithAngular.Configurations
{
    public class ValidateRequests
    {
       public bool ValidateSignature(string token, JwtOptions options, out JwtSecurityToken jwt, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtOptions:SecurityKey").Value));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,

                ValidateAudience = true,
                ValidAudience = options.Audience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                ClockSkew = TimeSpan.FromMinutes(1)
            };

            try
            {  
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                jwt = (JwtSecurityToken)validatedToken;
                if (jwt.ValidTo <= jwt.ValidFrom)
                {
                    throw new SecurityTokenInvalidLifetimeException("Token expirado.");
                }
                return true;
            }
            catch (SecurityTokenValidationException)
            {
                jwt = null;
                return false;
            }
            catch (ArgumentNullException)
            {
                jwt = null;
                return false;
            }
        }
    }
}
