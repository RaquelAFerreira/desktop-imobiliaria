using System.ComponentModel;

namespace AluguelImoveis.Models.Enums
{
    public enum TipoImovel
    {
        [Description("Apartamento")]
        Apartamento = 1,

        [Description("Casa")]
        Casa = 2,

        [Description("Sobrado")]
        Sobrado = 3,

        [Description("Kitnet")]
        Kitnet = 4,

        [Description("Loja Comercial")]
        LojaComercial = 5,

        [Description("Galpao Industrial")]
        GalpaoIndustrial = 6,

        [Description("Terreno")]
        Terreno = 7,

        [Description("Chácara")]
        Chacara = 8,

        [Description("Sítio")]
        Sitio = 9,

        [Description("Flat")]
        Flat = 10
    }
}