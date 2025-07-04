using AluguelImoveis.Helpers;
using AluguelImoveis.Models;
using AluguelImoveis.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AluguelImoveis.Views
{
    public partial class UpdateImovelView : Window
    {
        private readonly Imovel _imovel;
        private readonly IImovelHttpService _imovelService;

        public UpdateImovelView(Imovel imovel, IImovelHttpService imovelService)
        {
            InitializeComponent();
            _imovelService = imovelService;
            _imovel = imovel;
            FillFields();
        }

        private void FillFields()
        {
            CodigoBox.Text = _imovel.Codigo;
            TipoBox.Text = _imovel.Tipo;
            EnderecoBox.Text = _imovel.Endereco;
            ValorLocacaoBox.Text = _imovel.ValorLocacao.ToString();
            DisponivelBox.IsChecked = _imovel.Disponivel;
        }

        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                _imovel.Codigo = CodigoBox.Text;
                _imovel.Tipo = TipoBox.Text;
                _imovel.Endereco = EnderecoBox.Text.Trim();
                _imovel.ValorLocacao = decimal.Parse(ValorLocacaoBox.Text);
                _imovel.Disponivel = DisponivelBox.IsChecked ?? true;

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await _imovelService.UpdateAsync(_imovel);

                await ProcessResponse(response);
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
                MessageBox.Show($"Não foi possível atualizar o imóvel.\n{ex.Message}",
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
            if (string.IsNullOrWhiteSpace(CodigoBox.Text))
            {
                MessageBox.Show("O campo 'Código' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                CodigoBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(EnderecoBox.Text))
            {
                MessageBox.Show("O campo 'Endereço' é obrigatório.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                EnderecoBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TipoBox.Text))
            {
                MessageBox.Show("O campo 'Tipo' é obrigatório.",
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

        private async Task ProcessResponse(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    MessageBox.Show($"Imóvel atualizado com sucesso!",
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

                case HttpStatusCode.NotFound:
                    MessageBox.Show("Imóvel não encontrado no sistema.",
                                  "Não Encontrado",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    Close();
                    break;

                default:
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao atualizar imóvel!\n{errorContent}",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }

        private void ValorLocacaoBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateDecimal.DecimalOnly_PreviewTextInput(sender, e);
        }

        private void ValorLocacaoBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ValidateDecimal.DecimalOnly_Pasting(sender, e);
        }
    }
}