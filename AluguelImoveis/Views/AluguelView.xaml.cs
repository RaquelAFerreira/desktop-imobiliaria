using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class AlugueisView : Page
    {
        private IEnumerable<AluguelDetalhadoDto> todosAlugueis;

        public AlugueisView()
        {
            InitializeComponent();
            StatusComboBox.SelectedIndex = 0; // Seleciona "Todos" por padrão
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                todosAlugueis = await ApiService.GetAlugueisAsync();
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
            if (todosAlugueis == null) return;

            var alugueisFiltrados = todosAlugueis;
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
            var view = new CriarAluguelView();
            if (view.ShowDialog() == true)
            {
                _ = LoadDataAsync();
            }
        }

        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is AluguelDetalhadoDto aluguel)
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
                        await ApiService.ExcluirAluguelAsync(aluguel.AluguelId);
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