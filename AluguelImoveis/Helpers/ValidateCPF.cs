using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluguelImoveis.Helpers
{
    class ValidateCPF
    {
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
