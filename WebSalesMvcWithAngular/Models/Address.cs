using System.ComponentModel.DataAnnotations;

namespace WebSalesMvcWithAngular.Models
{
    [Help("Classe responsável por armazenar os dados consumidos e salvos da API ViaCep, implementação um pouco " +
        "diferente pois os dados vem de uma API externa, e alguns dados são armazenados conforme a necessidade." +
        "O atributo Número não vem da API, é um auxiliar opcional para salvar endereços específicos.")]
    public class Address
    {
        [Key]
        public int? AdressId { get; set; }
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Localidade { get; set; }
        public string? UF { get; set; }
        public string? DDD { get; set; }
        public int? Number { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}