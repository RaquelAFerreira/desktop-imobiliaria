namespace AluguelImoveis.Models.DTOs
{
    public class AluguelDto
    {
        public Guid AluguelId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int TotalDias { get; set; }
        public int DiasRestantes { get; set; }
        public ImovelDto Imovel { get; set; }
        public LocatarioDto Locatario { get; set; }
        public bool Disponivel { get; set; }

    }
}
