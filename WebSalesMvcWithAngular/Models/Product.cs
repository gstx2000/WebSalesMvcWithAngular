using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSalesMvcWithAngular.Models;

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
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]

        [Display(Name = "Categoria")]
        public Category? Category { get; set; }

        [Display(Name = "Preço")]
        public double Price { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
       
        [Display(Name = "Imagem")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Venda")]
        public List<SoldProduct>? SoldProducts { get; set; }

    }

}
