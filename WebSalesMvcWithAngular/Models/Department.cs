using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSalesMvc.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Name { get; set; }

        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        [NotMapped]
        public int NumberOfSellers => Sellers.Count;
        public string Address { get; set; }
        public string CNPJ { get; set; }
        public Department()
        {
        }
        public Department(int id, string name, string address, string cnpj)
        {
            Id = id;
            Name = name;
            Address = address;
            CNPJ = cnpj;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public decimal TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(seller =>  seller.TotalSales(initial, final));
        }
        public IEnumerable<Seller> GetSellers()
        {
            return Sellers.ToList();
        }


    }
}
