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

        [Display(Name = "Subcategoria")]
        public bool IsSubCategory { get; set; } = false;

        [Display(Name = "Produtos")]
       
        [JsonIgnore]
        public List<Product>? Products { get; set; } = new List<Product>();

        public Category? ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }

        public ICollection<Category>? SubCategories { get; set; }
        public Category()
        {
            SubCategories = new HashSet<Category>();
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}
