using System;
using System.Globalization;
using System.Windows.Data;

namespace AluguelImoveis.Helpers;

public class CPFFormatterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string cpf = value as string;

        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            return cpf;

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string formatted = value as string;
        if (formatted == null)
            return value;

        return formatted.Replace(".", "").Replace("-", "");
    }
}
