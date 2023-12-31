using System.ComponentModel.DataAnnotations;

namespace WebSalesMvc.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "As senhas devem ser iguais.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }

}
