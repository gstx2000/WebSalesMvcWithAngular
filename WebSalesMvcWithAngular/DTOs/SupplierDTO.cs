using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.DTOs
{
    public class SupplierDTO
    {
        public int? SupplierId { get; set; }
        public string? Name { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public List<Address>? Addresses { get; set; }
        public decimal? ShippingValue { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
        public SupplierType? SupplierType { get; set; }
        public int? DayToPay { get; set; }
        public decimal? SupplyPrice { get; set; } 

    }
}
