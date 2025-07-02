using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class AlugueisView : Page
    {
        private IEnumerable<AluguelDto> allAlugueis;
        private readonly IAluguelHttpService _aluguelService;
        private readonly IImovelHttpService _imovelService;
        private readonly ILocatarioHttpService _locatarioService;

        public AlugueisView(IAluguelHttpService aluguelService, IImovelHttpService imovelService, ILocatarioHttpService locatarioService)
        {
            InitializeComponent();
            _aluguelService = aluguelService;
            _imovelService = imovelService;
            _locatarioService = locatarioService;
            StatusComboBox.SelectedIndex = 0; // Seleciona "Todos" por padrão
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                allAlugueis = await _aluguelService.GetAllAsync();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar aluguéis: {ex.Message}", "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AplicarFiltro()
        {
            if (allAlugueis == null) return;

            var alugueisFiltrados = allAlugueis;
            var filtroSelecionado = (StatusComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString();

            switch (filtroSelecionado)
            {
                case "vigentes":
                    alugueisFiltrados = alugueisFiltrados
                        .Where(a => a.DataTermino.Date >= DateTime.Today)
                        .ToList();
                    break;
                case "encerrados":
                    alugueisFiltrados = alugueisFiltrados
                        .Where(a => a.DataTermino.Date < DateTime.Today)
                        .ToList();
                    break;
                case "todos":
                default:
                    break;
            }

            AlugueisList.ItemsSource = alugueisFiltrados;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void CadastrarAluguel_Click(object sender, RoutedEventArgs e)
        {
            var view = new CreateAluguelView(_aluguelService, _imovelService, _locatarioService);
            if (view.ShowDialog() == true)
            {
                _ = LoadDataAsync();
            }
        }

        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is AluguelDto aluguel)
            {
                var confirmar = MessageBox.Show(
                    $"Deseja realmente excluir o aluguel do imóvel {aluguel.Imovel.Codigo}?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmar == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _aluguelService.DeleteAsync(aluguel.AluguelId);
                        MessageBox.Show("Aluguel excluído com sucesso!");
                        await LoadDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir aluguel: {ex.Message}", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}