using Microsoft.IdentityModel.Tokens;

namespace WebSalesMvcWithAngular.Configurations
{
    public class JwtOptions
    {
        public string Issuer {get; set;}

        public string Audience { get; set;}

        public SigningCredentials SigninCredentials { get; set;}

        public int ExpirationDate { get; set;}  
    }
}
