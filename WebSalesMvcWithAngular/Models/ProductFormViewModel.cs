using System.Collections.Generic;

namespace WebSalesMvc.Models
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}

