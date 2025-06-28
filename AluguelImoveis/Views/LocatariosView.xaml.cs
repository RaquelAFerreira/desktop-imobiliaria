using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class LocatariosView : Page
    {
        private List<Locatario> todosLocatarios = new();

        public LocatariosView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            todosLocatarios = await ApiService.GetLocatariosAsync();
            LocatariosList.ItemsSource = todosLocatarios;
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CriarLocatarioView();
            form.ShowDialog();
            _ = LoadDataAsync();
        }

        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            var nome = NomeFiltroBox.Text.ToLower();
            var cpf = CpfFiltroBox.Text;

            var filtrados = todosLocatarios.Where(l =>
                (string.IsNullOrWhiteSpace(nome) || l.NomeCompleto.ToLower().Contains(nome)) &&
                (string.IsNullOrWhiteSpace(cpf) || l.CPF.Contains(cpf))
            ).ToList();

            LocatariosList.ItemsSource = filtrados;
        }
        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Locatario locatario)
            {
                var confirmar = MessageBox.Show(
                    $"Deseja realmente excluir o locatário de CPF {locatario.CPF}?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmar == MessageBoxResult.Yes)
                {
                    try
                    {
                        await ApiService.ExcluirLocatarioAsync(locatario.Id);
                        MessageBox.Show("Locatário excluído com sucesso!");
                        await LoadDataAsync();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Não foi possível excluir",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Locatário não encontrado",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao tentar excluir o locatário.", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
    }
}
