using WebSalesMvc.Models.Enums;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.DTOs
{
    public class SalesRecordDTO
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Seller { get; set; }
        public decimal? Amount { get; set; }
        public SaleStatus? Status { get; set; } 
        public PaymentMethod? PaymentMethod { get; set; } 
        public int? SellerId { get; set; }
        public List<SoldProduct>? SoldProducts { get; set; }
        public string? CustomerName { get; set; }
    }
}

