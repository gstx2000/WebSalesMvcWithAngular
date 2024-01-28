using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebSalesMvc.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        public  int DepartmentId { get; set; }
        public Department? Department { get; set; }

        [Display(Name = "Produto")]
       
        [JsonIgnore]
        public List<Product> Products { get; set; } = new List<Product>();
        public Category()
        {
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}
