using System.ComponentModel.DataAnnotations;
using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.Models
{
    [Help("Classe fornecedora dos produtos, pode fornecer mais de um produto." +
        " Classe auxiliar: ProductSupplier(Junction Table)")]
    public class Supplier
    {
        [Key]
        public int? SupplierId { get; set; }
        public required string Name { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<ProductSupplier>? Products { get; set; }
        public List<Address>? Addresses { get; set; } 
        public double? ShippingValue { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
        public SupplierType? SupplierType { get; set; }
        public int? DayToPay { get; set; }
    }
} 
