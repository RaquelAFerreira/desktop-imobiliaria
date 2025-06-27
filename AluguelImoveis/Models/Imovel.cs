
namespace AluguelImoveis.Models
{
    public class Imovel
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; } = String.Empty;
        public decimal ValorLocacao { get; set; }
        public bool Disponivel { get; set; } = true;
        public string Codigo { get; set; } = String.Empty;
    }
}
