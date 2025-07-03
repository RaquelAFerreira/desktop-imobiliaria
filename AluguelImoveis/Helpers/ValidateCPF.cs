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
    public static class ValidateCPF
    {
        private static readonly Regex DigitsOnly = new Regex(@"^\d+$");

        public static void Cpf_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !DigitsOnly.IsMatch(e.Text);
        }

        public static void Cpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string raw = GetDigits(textBox.Text);
                if (raw.Length > 11)
                    raw = raw.Substring(0, 11);

                string formatted = FormatCpf(raw);

                int digitsTyped = raw.Length;

                textBox.TextChanged -= Cpf_TextChanged;
                textBox.Text = formatted;
                textBox.SelectionStart = CalculateCaretIndex(formatted, digitsTyped);
                textBox.TextChanged += Cpf_TextChanged;
            }
        }

        public static void Cpf_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                if (!DigitsOnly.IsMatch(GetDigits(pasteText)))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static string GetDigits(string input) =>
            Regex.Replace(input ?? "", @"\D", "");

        private static string FormatCpf(string digits)
        {
            if (digits.Length <= 3)
                return digits;
            if (digits.Length <= 6)
                return $"{digits.Substring(0, 3)}.{digits.Substring(3)}";
            if (digits.Length <= 9)
                return $"{digits.Substring(0, 3)}.{digits.Substring(3, 3)}.{digits.Substring(6)}";
            if (digits.Length <= 11)
                return $"{digits.Substring(0, 3)}.{digits.Substring(3, 3)}.{digits.Substring(6, 3)}-{digits.Substring(9)}";

            return digits;
        }

        private static int CalculateCaretIndex(string formatted, int digitsCount)
        {
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

        public static bool IsValidCPF(object? value)
        {
            if (value is not string cpfString)
                return false;

            cpfString = new string(cpfString.Where(char.IsDigit).ToArray());

            if (cpfString.Length != 11 || cpfString.All(d => d == cpfString[0]))
                return false;

            var firstMultipliers = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondMultipliers = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var partialCpf = cpfString.Substring(0, 9);
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(partialCpf[i].ToString()) * firstMultipliers[i];

            var remainder = sum % 11;
            var firstDigit = remainder < 2 ? 0 : 11 - remainder;

            partialCpf += firstDigit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(partialCpf[i].ToString()) * secondMultipliers[i];

            remainder = sum % 11;
            var secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return cpfString.EndsWith(firstDigit.ToString() + secondDigit.ToString());
        }
    }
}