using System.ComponentModel.DataAnnotations;


namespace WebSalesMvcWithAngular.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
