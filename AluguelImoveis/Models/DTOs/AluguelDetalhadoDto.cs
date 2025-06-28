using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluguelImoveis.Models.DTOs
{
    public class AluguelDetalhadoDto
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

    public class ImovelDto
    {
        public string Endereco { get; set; }
        public string Tipo { get; set; }
        public string Codigo { get; set; }

        public decimal ValorLocacao { get; set; }

    }

    public class LocatarioDto
    {
        public string NomeCompleto { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }

    }
}
