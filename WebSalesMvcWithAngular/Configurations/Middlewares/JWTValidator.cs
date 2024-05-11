using Microsoft.AspNetCore.Mvc;
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
            string route = context.Request.Path;
            string authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

                    if (IsLogoutRoute(context.GetEndpoint()))
                    {
                        context.Items["UserId"] = userIdClaim?.Value;
                        await next(context);
                        _logger.LogInformation($"UserId: { userIdClaim?.Value }");

                        return;
                    }
                
                    if (_validator.ValidateSignature(token, _jwtOptions, out jwtToken, _conf))
                    {
                        await next(context);
                        _logger.LogInformation($"Token validated successfully {jwtToken}");
                    }
                    else
                    {
                        _logger.LogError("Token validation failed for endpoint.");
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                catch (Exception)
                {
                    //context.Response.StatusCode = 401;
                    //await context.Response.WriteAsync("Token inválido ou expirado.");
                    //return;
                }
            }
        }

        private bool IsLogoutRoute(Endpoint endpoint)
        {
            if (endpoint?.Metadata.GetMetadata<RouteAttribute>()?.Template == "logout")
            {
                return true;
            }
            return false;
        }
    }
}
