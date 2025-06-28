using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AluguelImoveis.Views
{
    public partial class AlugueisView : Page
    {
        public AlugueisView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                if (ToggleAtivos?.IsChecked == true)
                {
                    var ativos = await ApiService.GetAlugueisAtivosAsync();
                    AlugueisList.ItemsSource = ativos;
                }
                else
                {
                    var alugueis = await ApiService.GetAlugueisAsync();
                    AlugueisList.ItemsSource = alugueis;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar aluguéis: {ex.Message}");
            }
        }

        private void ToggleAtivos_Checked(object sender, RoutedEventArgs e)
        {
            _ = LoadDataAsync();
        }

        private void ToggleAtivos_Unchecked(object sender, RoutedEventArgs e)
        {
            _ = LoadDataAsync();
        }

        private void CadastrarAluguel_Click(object sender, RoutedEventArgs e)
        {
            var view = new CriarAluguelView();
            view.ShowDialog();
            _ = LoadDataAsync();
        }

        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is AluguelDetalhadoDto aluguel)
            {
                var confirmar = MessageBox.Show(
                    $"Deseja realmente excluir esse aluguel?",
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
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Aluguel não encontrado",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao tentar excluir o aluguel.", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
