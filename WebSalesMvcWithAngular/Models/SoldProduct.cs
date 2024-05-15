using System.ComponentModel.DataAnnotations;
using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Models
{
  public class SoldProduct
    {
        [Key]
        public int Id { get; set; }

        public int? SalesRecordId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
        public Product? Product { get; set; }

        public decimal? Price { get; set; }
        public decimal? AcquisitionCost { get; set; }
        public decimal? Margin { get; set; }

        public decimal CalculateMargin()
        {
            if (Quantity == 0 || Quantity == null || AcquisitionCost == null || AcquisitionCost == 0)
            {
                return 0;
            }
            return (decimal)((Price - AcquisitionCost) / AcquisitionCost * 100);
        }

    }
}
