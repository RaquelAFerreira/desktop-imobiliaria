using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class LocatariosView : Page
    {
        private List<Locatario> allLocatarios = new();

        private readonly ILocatarioHttpService _locatarioService;

        public LocatariosView(ILocatarioHttpService locatarioService)
        {
            InitializeComponent();
            _locatarioService = locatarioService;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            allLocatarios = await _locatarioService.GetAllAsync();
            LocatariosList.ItemsSource = allLocatarios;
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CreateLocatarioView(_locatarioService);
            form.ShowDialog();
            _ = LoadDataAsync();
        }

        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            var nome = NomeFiltroBox.Text.ToLower();
            var cpf = CpfFiltroBox.Text;

            var filtered = allLocatarios.Where(l =>
                (string.IsNullOrWhiteSpace(nome) || l.NomeCompleto.ToLower().Contains(nome)) &&
                (string.IsNullOrWhiteSpace(cpf) || l.CPF.Contains(cpf))
            ).ToList();

            LocatariosList.ItemsSource = filtered;
        }
        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Locatario locatario)
            {
                var confirm = MessageBox.Show(
                    $"Deseja realmente excluir o locat�rio de CPF {locatario.CPF}?",
                    "Confirma��o",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _locatarioService.DeleteAsync(locatario.Id);
                        MessageBox.Show("Locat�rio exclu�do com sucesso!");
                        await LoadDataAsync();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "N�o foi poss�vel excluir",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Locat�rio n�o encontrado",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao tentar excluir o locat�rio.", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
    }
}
