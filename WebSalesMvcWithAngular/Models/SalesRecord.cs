using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSalesMvc.Models.Enums;
using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvc.Models
{
    public class SalesRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Valor")]

        public double Amount { get; set; }

        [Required]
        [Display(Name = "Status")]
        public SaleStatus Status { get; set; }

        [Required]
        [Display(Name = "Forma de pagamento")]
        public PaymentMethod PaymentForm { get; set; }

        [Display(Name = "Vendedor")]
        public Seller Seller { get; set; }
        
        public int SellerId { get; set; }

        [NotMapped]

        [Display(Name = "Produtos")]
        public ICollection<Product> Products { get; set; }

    [Display(Name = "Nome do cliente")]
        public string CustomerName { get; set; }


        public void UpdateAmount()
        {
            Amount = Products.Sum(product => product.Price);
        }


        public SalesRecord()
        {
            Products = new List<Product>();

        }
        public SalesRecord(int id, DateTime date, SaleStatus status, Seller seller, int sellerId, ICollection<Product> products, string customerName)
        {
            Id = id;
            Date = date;
            Status = status;
            Seller = seller;
            SellerId = sellerId;
            Products = products ?? new List<Product>(); 
            CustomerName = customerName;
        }
    }
}
