using System.Collections.Generic;
using WebSalesMvc.Models;
using static WebSalesMvc.Models.SalesRecord;

namespace WebSalesMvc.Models
{
    public class SalesRecordsCreateViewModel
    {
        public List<Seller> Sellers { get; set; }
        public SalesRecord SalesRecord { get; set; }
        public ICollection<Product> Products { get; set; }
        public List<SelectedProduct> SelectedProducts { get; set; }
        public string SelectedProductsJson { get; set; }


    }
}

