using AluguelImoveis.Helpers;
using AluguelImoveis.Models;
using AluguelImoveis.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AluguelImoveis.Views
{
    public partial class CreateLocatarioView : Window
    {
        private readonly ILocatarioHttpService _locatarioService;

        public CreateLocatarioView(ILocatarioHttpService locatarioService)
        {
            InitializeComponent();
            _locatarioService = locatarioService;
        }

        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                var locatario = new Locatario
                {
                    NomeCompleto = NomeCompletoBox.Text.Trim(),
                    Telefone = TelefoneBox.Text,
                    CPF = Regex.Replace(CPFBox.Text, @"[^\d]", "")
                };

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await _locatarioService.CreateAsync(locatario);

                await ProcessResponse(response, locatario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível criar um locatário.\n{ex.Message}",
                              "Erro",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            finally
            {
                SalvarButton.IsEnabled = true;
                SalvarButton.Content = "Salvar";
            }
        }
        private bool ValidateFields()
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

            if (string.IsNullOrWhiteSpace(TelefoneBox.Text))
            {
                MessageBox.Show("O campo 'Telefone' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                TelefoneBox.Focus();
                return false;
            }
            else if (!ValidatePhoneNumber.IsValidPhoneNumber(TelefoneBox.Text))
            {
                MessageBox.Show("Digite um telefone válido.",
                                "Validação",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                TelefoneBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CPFBox.Text))
            {
                MessageBox.Show("O campo 'CPF' é obrigatório.",
                                "Validação",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                CPFBox.Focus();
                return false;
            }
            else if (!ValidateCPF.IsValidCPF(CPFBox.Text))
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
        private async Task ProcessResponse(HttpResponseMessage response, Locatario imovel)
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
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Dados inválidos:\n{errorDetails}",
                                  "Erro de Validação",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    break;
                default:
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar locatário!\n{errorContent}",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }

        private void TelefoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatePhoneNumber.Phone_PreviewTextInput(sender, e);
        }

        private void TelefoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePhoneNumber.Phone_TextChanged(sender, e);
        }

        private void TelefoneBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ValidatePhoneNumber.Phone_Pasting(sender, e);
        }

        private void CpfBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateCPF.Cpf_PreviewTextInput(sender, e);
        }

        private void CpfBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCPF.Cpf_TextChanged(sender, e);
        }

        private void CpfBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ValidateCPF.Cpf_Pasting(sender, e);
        }


    }
}
