using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluguelImoveis.Models
{
    internal class Aluguel
    {
        public Guid Id { get; set; }
        public Guid ImovelId { get; set; }
        public Guid LocatarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}
