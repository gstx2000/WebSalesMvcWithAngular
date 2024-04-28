using System.ComponentModel.DataAnnotations;

namespace WebSalesMvcWithAngular.DTOs.Requests
{ 
    public class PasswordRecoveryRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public required string Email { get; set; }
    }
}
