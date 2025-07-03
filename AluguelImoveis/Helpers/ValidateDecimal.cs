using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace AluguelImoveis.Helpers
{
    public static class ValidateDecimal
    {
        // Permite apenas números com até 2 casas decimais (ponto ou vírgula)
        private static readonly Regex DecimalRegex = new Regex(@"^\d*(?:[.,]\d{0,2})?$");

        public static void DecimalOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string currentText = textBox.Text;
                int caretIndex = textBox.CaretIndex;

                // Texto simulado após digitação
                string newText = currentText.Insert(caretIndex, e.Text);

                // Substitui vírgula por ponto ou vice-versa se quiser unificar
                newText = newText.Replace(',', '.');

                e.Handled = !DecimalRegex.IsMatch(newText);
            }
        }

        public static void DecimalOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = ((string)e.DataObject.GetData(typeof(string))).Replace(',', '.');
                if (!DecimalRegex.IsMatch(pasteText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
