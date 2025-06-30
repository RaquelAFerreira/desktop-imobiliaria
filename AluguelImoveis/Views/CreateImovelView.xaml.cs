using AluguelImoveis.Models;
using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace AluguelImoveis.Views
{
    public partial class CreateImovelView : Window
    {
        private readonly IImovelHttpService _imovelService;

        public CreateImovelView(IImovelHttpService imovelService)
        {
            InitializeComponent();
            _imovelService = imovelService;
        }
        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                var imovel = new Imovel
                {
                    Codigo = CodigoBox.Text,
                    Tipo = TipoBox.Text,
                    Endereco = EnderecoBox.Text.Trim(),
                    ValorLocacao = decimal.Parse(ValorLocacaoBox.Text),
                    Disponivel = DisponivelBox.IsChecked ?? true
                };

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await _imovelService.CreateAsync(imovel);

                await ProcessResponse(response, imovel);
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um valor numérico válido para o campo 'Valor Locação'.",
                              "Formato Inválido",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                ValorLocacaoBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível criar um imóvel.",
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
            if (string.IsNullOrWhiteSpace(EnderecoBox.Text))
            {
                MessageBox.Show("O campo 'Endereço' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                EnderecoBox.Focus();
                return false;
            }

            if (TipoBox.SelectedItem == null)
            {
                MessageBox.Show("Selecione um tipo de imóvel.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                TipoBox.Focus();
                return false;
            }

            if (!decimal.TryParse(ValorLocacaoBox.Text, out _) || decimal.Parse(ValorLocacaoBox.Text) <= 0)
            {
                MessageBox.Show("Insira um valor válido para locação (maior que zero).",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                ValorLocacaoBox.Focus();
                return false;
            }

            return true;
        }

        private async Task ProcessResponse(HttpResponseMessage response, Imovel imovel)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    await response.Content.ReadFromJsonAsync<Imovel>();
                    MessageBox.Show($"Imóvel cadastrado com sucesso!",
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
                    MessageBox.Show($"Erro ao cadastrar imóvel!",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }
    }
}