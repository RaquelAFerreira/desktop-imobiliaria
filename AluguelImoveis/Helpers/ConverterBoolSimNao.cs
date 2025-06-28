using System.Globalization;
using System.Windows.Data;

namespace AluguelImoveis.Converters
{
    internal class ConverterBoolSimNao : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
            {
                return boolean ? "Sim" : "Não";
            }
            return "Não";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().ToLower() == "sim";
        }
    }
}