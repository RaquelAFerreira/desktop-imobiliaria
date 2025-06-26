using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System.Net.Http;
using System.Windows;

namespace AluguelImoveis.Views
{
    public partial class CriarLocatarioView : Window
    {
        public CriarLocatarioView()
        {
            InitializeComponent();
        }

        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            var locatario = new Locatario
            {
                NomeCompleto = NomeCompletoBox.Text,
                Telefone = TelefoneBox.Text,
                CPF = CPFBox.Text
            };

            try
            {
                HttpResponseMessage response = await ApiService.CriarLocatarioAsync(locatario);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Locatário cadastrado com sucesso!");
                    Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar locatário:\n{error}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado:\n{ex.Message}", "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
