using System.ComponentModel.DataAnnotations;

namespace WebSalesMvcWithAngular.Models
{
    public class Customer
    {
        [Key]
        public int? CustomerId { get; set; }
        public string? CNPJ { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Phone { get; set; }
        public required string  FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<Address>? Adresses { get; set; }

    }
}
