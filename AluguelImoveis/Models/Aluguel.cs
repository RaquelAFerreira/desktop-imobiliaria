namespace AluguelImoveis.Models
{
    public class Aluguel
    {
        public Guid Id { get; set; }
        public Guid ImovelId { get; set; }
        public Guid LocatarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}
