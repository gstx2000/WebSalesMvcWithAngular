using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace WebSalesMvcWithAngular.Configurations.Middlewares
{
    public class JWTValidator : IMiddleware
    {
        private readonly ValidateRequests _validator;
        private readonly IConfiguration _conf;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<JWTValidator> _logger;
        public JWTValidator(ValidateRequests validator, IConfiguration conf, IOptions<JwtOptions> jwtOptions, ILogger<JWTValidator> logger)
        {
            _validator = validator;
            _conf = conf;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    if (!_validator.ValidateSignature(token, _jwtOptions, out jwtToken, _conf))
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }
            }
               await next(context);
            
        }
    }
}
