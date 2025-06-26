using AluguelImoveis.Helpers;
using AluguelImoveis.Models;
using AluguelImoveis.Models.Enums;
using AluguelImoveis.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace AluguelImoveis.Views
{
    public partial class CriarImovelView : Window
    {
        public CriarImovelView()
        {
            InitializeComponent();
            TipoBox.ItemsSource = Enum.GetValues(typeof(TipoImovel))
              .Cast<TipoImovel>()
              .Select(e => new { Value = (int)e, Description = EnumHelper.GetEnumDescription(e) })
              .ToList();

            TipoBox.DisplayMemberPath = "Description";
            TipoBox.SelectedValuePath = "Value";
        }

        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // Validação inicial dos campos
            if (!ValidarCampos())
            {
                return;
            }

            try
            {
                var imovel = new Imovel
                {
                    Tipo = (int)TipoBox.SelectedValue,
                    Endereco = EnderecoBox.Text.Trim(),
                    ValorLocacao = decimal.Parse(ValorLocacaoBox.Text),
                    Disponivel = DisponivelBox.IsChecked ?? true
                };

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await ApiService.CriarImovelAsync(imovel);

                await ProcessarResposta(response, imovel);
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um valor numérico válido para o campo 'Valor Locação'.",
                              "Formato Inválido",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                ValorLocacaoBox.Focus();
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Falha na conexão com a API:\n{httpEx.Message}",
                              "Erro de Conexão",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado:\n{ex.Message}",
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

        private async Task ProcessarResposta(HttpResponseMessage response, Imovel imovel)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    var imovelCriado = await response.Content.ReadFromJsonAsync<Imovel>();
                    MessageBox.Show($"Imóvel cadastrado com sucesso!\nID: {imovelCriado.Id}",
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

                case HttpStatusCode.Conflict:
                    MessageBox.Show("Já existe um imóvel com estas características cadastrado.",
                                  "Conflito",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    break;

                case HttpStatusCode.Unauthorized:
                    MessageBox.Show("Acesso não autorizado. Por favor, faça login novamente.",
                                  "Não Autorizado",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;

                default:
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar imóvel:\n{response.StatusCode}\n{errorContent}",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }
    }
}