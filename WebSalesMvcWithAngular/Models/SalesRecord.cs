using System.ComponentModel.DataAnnotations;
using WebSalesMvc.Models.Enums;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvc.Models
{
    public class SalesRecord
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data")]
        public DateTime? Date { get; set; }

        public Seller? Seller { get; set; }
       
        [Required]
        [Display(Name = "Valor")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Status")]
        public SaleStatus Status { get; set; }

        [Required]
        [Display(Name = "Forma de pagamento")]
        public PaymentMethod PaymentMethod { get; set; }

        public int SellerId { get; set; }

        [Display(Name = "Produtos")]
        public List<SoldProduct>? SoldProducts { get; set; }

        [Display(Name = "Nome do cliente")]
        public string? CustomerName { get; set; }

        [Display(Name = "Lucro")]
        public decimal? Profit  { get; set; }
        public SalesRecord()
        {
            SoldProducts = new List<SoldProduct>();

        }
        public SalesRecord(int id, DateTime date, SaleStatus status, int sellerId, List<SoldProduct> soldProducts, string customerName)
        {
            Id = id;
            Date = date;
            Status = status;
            SellerId = sellerId;
            SoldProducts = soldProducts ?? new List<SoldProduct>();
            CustomerName = customerName;
        }
    }
}
