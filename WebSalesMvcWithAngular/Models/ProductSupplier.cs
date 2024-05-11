﻿using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Models
{
    public class ProductSupplier
    {
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public double? SupplyPrice { get; set; } 
    }
}
