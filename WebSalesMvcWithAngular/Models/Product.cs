using System.ComponentModel.DataAnnotations;

namespace WebSalesMvc.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public required string Name { get; set; }

        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        [Display(Name = "Categoria")]
        public Category? Category { get; set; }

        [Display(Name = "Preço")]
        public double Price { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Departamento")]
        public Department? Department { get; set; }
       
        public int DepartmentId { get; set; }
      
        [Display(Name = "Imagem")]
        public string? ImageUrl { get; set; }


    }

}
