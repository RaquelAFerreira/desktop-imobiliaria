using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AluguelImoveis.Views
{
    public partial class CriarAluguelView : Window
    {
        public CriarAluguelView()
        {
            InitializeComponent();
            _ = CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            try
            {
                var imoveis = await ApiService.GetImoveisDisponiveisAsync();
                ImovelComboBox.ItemsSource = imoveis.Select(i => new
                {
                    Id = i.Id,
                    Descricao = i.Codigo
                }).ToList();

                var locatarios = await ApiService.GetLocatariosAsync();
                LocatarioComboBox.ItemsSource = locatarios.Select(l => new
                {
                    Id = l.Id,
                    Descricao = $"{l.CPF} - {l.NomeCompleto}"
                }).ToList();
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Falha ao carregar dados:\n{httpEx.Message}",
                              "Erro de Conexão",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado:\n{ex.Message}",
                              "Erro",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Close();
            }
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
                var aluguel = new Aluguel
                {
                    ImovelId = (Guid)ImovelComboBox.SelectedValue,
                    LocatarioId = (Guid)LocatarioComboBox.SelectedValue,
                    DataInicio = DataInicioPicker.SelectedDate ?? DateTime.MinValue,
                    DataTermino = DataTerminoPicker.SelectedDate ?? DateTime.MinValue
                };

                SalvarButton.IsEnabled = false;
                SalvarButton.Content = "Salvando...";

                HttpResponseMessage response = await ApiService.CriarAluguelAsync(aluguel);

                await ProcessarResposta(response, aluguel);
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
            if (ImovelComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selecione um imóvel.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                ImovelComboBox.Focus();
                return false;
            }

            if (LocatarioComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selecione um locatário.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                LocatarioComboBox.Focus();
                return false;
            }

            if (DataInicioPicker.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data de início.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                DataInicioPicker.Focus();
                return false;
            }

            if (DataTerminoPicker.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data de término.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                DataTerminoPicker.Focus();
                return false;
            }

            if (DataInicioPicker.SelectedDate > DataTerminoPicker.SelectedDate)
            {
                MessageBox.Show("A data de início não pode ser maior que a data de término.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                DataInicioPicker.Focus();
                return false;
            }

            return true;
        }

        private async Task ProcessarResposta(HttpResponseMessage response, Aluguel aluguel)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    var aluguelCriado = await response.Content.ReadFromJsonAsync<Aluguel>();
                    MessageBox.Show($"Aluguel cadastrado com sucesso!",
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
                    MessageBox.Show($"Erro ao cadastrar aluguel:\n{response.StatusCode}\n{errorContent}",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }
    }
}