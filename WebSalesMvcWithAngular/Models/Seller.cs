using System.ComponentModel.DataAnnotations;

namespace WebSalesMvc.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Telefone inválido.")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Email inválido.")]
        public string Email   { get; set; }

        [Display(Name = "Salário")]
        [Required(ErrorMessage = "Salário obrigatório.")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
         public double BaseSalary { get; set; }

        [Required(ErrorMessage = "Insira a data de nascimento antes de continuar.")]
        [Display(Name = "Data de nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Departamento")]
        public Department Department { get; set; }
        public  int DepartmentId  { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }

        public Seller(int id, string name, string email, double baseSalary, DateTime birthdate, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            Birthdate = birthdate;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
