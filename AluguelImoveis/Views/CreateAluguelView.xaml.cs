using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
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
    public partial class CreateAluguelView : Window
    {
        private readonly IAluguelHttpService _aluguelService;
        private readonly IImovelHttpService _imovelService;
        private readonly ILocatarioHttpService _locatarioService;

        public CreateAluguelView(IAluguelHttpService aluguelService, IImovelHttpService imovelService, ILocatarioHttpService locatarioService)
        {
            InitializeComponent();
            _aluguelService = aluguelService;
            _imovelService = imovelService;
            _locatarioService = locatarioService;

            _ = CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            try
            {
                var imoveis = await _imovelService.GetDisponiveisAsync();
                if (imoveis == null || !imoveis.Any())
                {
                    MessageBox.Show("Não há imóveis disponíveis para aluguel no momento.",
                                  "Informação",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    ImovelComboBox.IsEnabled = false;
                }
                else
                {
                    ImovelComboBox.ItemsSource = imoveis.Select(i => new
                    {
                        Id = i.Id,
                        Descricao = i.Codigo
                    }).ToList();
                }

                var locatarios = await _locatarioService.GetAllAsync();
                if (locatarios == null || !locatarios.Any())
                {
                    MessageBox.Show("Não há locatários cadastrados no sistema.",
                                  "Informação",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    LocatarioComboBox.IsEnabled = false;
                }
                else
                {
                    LocatarioComboBox.ItemsSource = locatarios.Select(l => new
                    {
                        Id = l.Id,
                        Descricao = $"{l.CPF} - {l.NomeCompleto}"
                    }).ToList();
                }

                // Desabilitar botão de salvar se não houver imóveis ou locatários
                SalvarButton.IsEnabled = ImovelComboBox.IsEnabled && LocatarioComboBox.IsEnabled;
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
            if (!ValidateFields())
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

                HttpResponseMessage response = await _aluguelService.CreateAsync(aluguel);

                await ProcessResponse(response, aluguel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível criar um registro de aluguel.\n{ex.Message}",
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

        private bool ValidateFields()
        {
            if (!ImovelComboBox.IsEnabled)
            {
                MessageBox.Show("Não há imóveis disponíveis para seleção.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

            if (!LocatarioComboBox.IsEnabled)
            {
                MessageBox.Show("Não há locatários disponíveis para seleção.",
                              "Validação",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

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

        private async Task ProcessResponse(HttpResponseMessage response, Aluguel aluguel)
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
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Dados inválidos:\n{errorDetails}",
                                  "Erro de Validação",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    break;

                default:
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar aluguel!\n{errorContent}",
                                  "Erro na API",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    break;
            }
        }
    }
}