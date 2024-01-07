using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSalesMvc.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Name { get; set; }

        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        [NotMapped]
        public int NumberOfSellers => Sellers.Count;


        public Department()
        {
        }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(seller =>  seller.TotalSales(initial, final));
        }
        public IEnumerable<Seller> GetSellers()
        {
            return Sellers.ToList();
        }


    }
}
