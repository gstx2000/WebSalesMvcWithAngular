using System.ComponentModel.DataAnnotations;
namespace WebSalesMvcWithAngular.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email inválido.")]

        public required string Email { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Senhas estão diferentes, tente novamente.")]
        public required string ConfirmPassword { get; set; }
    }
}