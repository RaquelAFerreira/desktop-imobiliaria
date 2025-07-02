using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AluguelImoveis.Helpers
{
    public static class ValidatePhoneNumber
    {
        private static readonly Regex DigitsOnly = new Regex(@"^\d+$");

        public static void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsDigit(e.Text);
        }

        public static void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string raw = GetDigits(textBox.Text);

                if (raw.Length > 11)
                    raw = raw.Substring(0, 11);

                string formatted = FormatPhoneNumber(raw);

                int oldSelectionStart = textBox.SelectionStart;
                textBox.TextChanged -= Phone_TextChanged; // evitar loop

                textBox.Text = formatted;
                textBox.SelectionStart = CalculateCaretIndex(formatted, raw.Length);
                textBox.TextChanged += Phone_TextChanged;
            }
        }

        public static void Phone_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                if (!DigitsOnly.IsMatch(GetDigits(pastedText)))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsDigit(string text) =>
            DigitsOnly.IsMatch(text);

        private static string GetDigits(string text) =>
            Regex.Replace(text ?? "", @"\D", "");

        private static string FormatPhoneNumber(string digits)
        {
            if (string.IsNullOrEmpty(digits)) return "";

            if (digits.Length <= 2)
                return $"({digits}";
            if (digits.Length <= 7)
                return $"({digits.Substring(0, 2)}) {digits.Substring(2)}";
            if (digits.Length <= 11)
                return $"({digits.Substring(0, 2)}) {digits.Substring(2, 5)}-{digits.Substring(7)}";

            return digits;
        }

        private static int CalculateCaretIndex(string formatted, int digitsCount)
        {
            // Garante que o cursor fique no fim dos dígitos válidos
            // Exemplo: digitar 5 números -> cursor após o 5º número
            int count = 0;
            for (int i = 0; i < formatted.Length; i++)
            {
                if (char.IsDigit(formatted[i]))
                    count++;

                if (count == digitsCount)
                    return i + 1;
            }
            return formatted.Length;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return digitsOnly.Length == 11;
        }
    }
}
