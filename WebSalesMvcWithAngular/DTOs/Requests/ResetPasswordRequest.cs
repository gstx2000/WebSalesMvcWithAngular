namespace WebSalesMvcWithAngular.DTOs.Requests
{
    public class ResetPasswordRequest : RegisterRequest
    {
        public string token { get; set; }
    }
}
