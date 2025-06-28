using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class ImoveisView : Page
    {        
        private IEnumerable<Imovel> todosImoveis;

        public ImoveisView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            todosImoveis = await ApiService.GetImoveisAsync();
            ImoveisList.ItemsSource = todosImoveis;
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CriarImovelView();
            form.ShowDialog();
            _ = LoadDataAsync();
        }
        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            var imoveis = todosImoveis; 

            // Filtro por disponibilidade
            if (DisponivelComboBox.SelectedItem != null)
            {
                string disponivelSelecionado = (DisponivelComboBox.SelectedItem as ComboBoxItem).Content.ToString();

                if (disponivelSelecionado == "Sim")
                    imoveis = imoveis.Where(i => i.Disponivel).ToList();
                else if (disponivelSelecionado == "Não")
                    imoveis = imoveis.Where(i => !i.Disponivel).ToList();
            }

            if (TipoComboBox.SelectedItem != null)
            {
                string tipoSelecionado = (TipoComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (!string.IsNullOrEmpty(tipoSelecionado))
                    imoveis = imoveis.Where(i => i.Tipo == tipoSelecionado).ToList();
            }

            if (!string.IsNullOrEmpty(ValorMinBox.Text) && decimal.TryParse(ValorMinBox.Text, out decimal valorMin))
                imoveis = imoveis.Where(i => i.ValorLocacao >= valorMin).ToList();

            if (!string.IsNullOrEmpty(ValorMaxBox.Text) && decimal.TryParse(ValorMaxBox.Text, out decimal valorMax))
                imoveis = imoveis.Where(i => i.ValorLocacao <= valorMax).ToList();

            ImoveisList.ItemsSource = imoveis;
        }

        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Imovel imovel)
            {
                var confirmar = MessageBox.Show(
                    $"Deseja realmente excluir o imóvel de código {imovel.Codigo}?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmar == MessageBoxResult.Yes)
                {
                    try
                    {
                        await ApiService.ExcluirImovelAsync(imovel.Id);
                        MessageBox.Show("Imóvel excluído com sucesso!");
                        await LoadDataAsync();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Não foi possível excluir",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Imóvel não encontrado",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao tentar excluir o imóvel.", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Imovel imovel)
            {
                var editView = new EditarImovelView(imovel);
                if (editView.ShowDialog() == true)
                {
                    _ = LoadDataAsync();
                }
            }
        }
    }
}
