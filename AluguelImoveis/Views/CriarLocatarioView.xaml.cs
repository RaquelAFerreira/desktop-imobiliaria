using AluguelImoveis.Helpers;
using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
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
            if (!ValidarCampos())
            {
                return;
            }

            try
            {
                var locatario = new Locatario
                {
                    NomeCompleto = NomeCompletoBox.Text.Trim(),
                    Telefone = Regex.Replace(TelefoneBox.Text, @"[^\d]", ""),
                    CPF = Regex.Replace(CPFBox.Text, @"[^\d]", "")
                };

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await ApiService.CriarLocatarioAsync(locatario);

                await ProcessarResposta(response, locatario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível criar um locatário.",
                              "Erro",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            finally
            {
                // Reabilitar botão após a operação
                SalvarButton.IsEnabled = true;
                SalvarButton.Content = "Salvar";
            }
        }
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(NomeCompletoBox.Text))
            {
                MessageBox.Show("O campo 'Nome Completo' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                NomeCompletoBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TelefoneBox.Text) || !Regex.IsMatch(TelefoneBox.Text, @"^\d+$"))
            {
                MessageBox.Show("O campo 'Telefone' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                TelefoneBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CPFBox.Text) || !Regex.IsMatch(CPFBox.Text, @"^\d+$"))
            {
                MessageBox.Show("O campo 'CPF' é obrigatório.",
                                "Validação",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                CPFBox.Focus();
                return false;
            }
            else if (!ValidarCPF.CPFValido(CPFBox.Text))
            {
                MessageBox.Show("O campo 'CPF' deve ser válido (conter 11 dígitos).",
                                "Validação",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                                CPFBox.Focus();
                return false;
            }

                return true;
        }
        private async Task ProcessarResposta(HttpResponseMessage response, Locatario imovel)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    var imovelCriado = await response.Content.ReadFromJsonAsync<Imovel>();
                    MessageBox.Show($"Locatário cadastrado com sucesso!",
                                  "Sucesso",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                    break;

                case HttpStatusCode.BadRequest:
                    var detalhesErro = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Dados inválidos:\n{detalhesErro}",
                                  "Erro de Validação",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    break;
                default:
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar locatário!",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }
    }
}
