using System.ComponentModel.DataAnnotations;

namespace WebSalesMvc.Models
{
    public class PasswordRecoveryViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
