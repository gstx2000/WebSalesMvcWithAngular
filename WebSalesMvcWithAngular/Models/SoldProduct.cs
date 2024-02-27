using System.ComponentModel.DataAnnotations;

namespace WebSalesMvcWithAngular.Models
{
  public class SoldProduct
    {
        [Key]
        public int Id { get; set; }

        public int? SalesRecordId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    } 
}
