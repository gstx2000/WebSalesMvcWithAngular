using WebSalesMvcWithAngular.DTOs.Requests;
using WebSalesMvcWithAngular.DTOs.Responses;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<RegisterUserResponse> RegisterUser(RegisterRequest registerRequest);

        Task<LoginUserResponse> LoginUser(LoginRequest loginRequest);
    }
}
