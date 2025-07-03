using AluguelImoveis.Models;
using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class ImoveisView : Page
    {        
        private IEnumerable<Imovel> allImoveis;
        private readonly IImovelHttpService _imovelService;

        public ImoveisView(IImovelHttpService imovelService)
        {
            InitializeComponent();
            _imovelService = imovelService;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            allImoveis = await _imovelService.GetAllAsync();
            ImoveisList.ItemsSource = allImoveis;
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CreateImovelView(_imovelService);
            form.ShowDialog();
            _ = LoadDataAsync();
        }
        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            var imoveis = allImoveis; 

            if (DisponivelComboBox.SelectedItem != null)
            {
                string selectedDisponivel = (DisponivelComboBox.SelectedItem as ComboBoxItem).Content.ToString();

                if (selectedDisponivel == "Sim")
                    imoveis = imoveis.Where(i => i.Disponivel).ToList();
                else if (selectedDisponivel == "Não")
                    imoveis = imoveis.Where(i => !i.Disponivel).ToList();
            }

            if (TipoComboBox.SelectedItem != null)
            {
                string selectedTipo = (TipoComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (!string.IsNullOrEmpty(selectedTipo))
                    imoveis = imoveis.Where(i => i.Tipo == selectedTipo).ToList();
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
                var confirm = MessageBox.Show(
                    $"Deseja realmente excluir o imóvel de código {imovel.Codigo}?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _imovelService.DeleteAsync(imovel.Id);
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
                        MessageBox.Show($"Ocorreu um erro ao tentar excluir o imóvel.\n{ex.Message}", "Erro",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Imovel imovel)
            {
                var editView = new UpdateImovelView(imovel, _imovelService);
                if (editView.ShowDialog() == true)
                {
                    _ = LoadDataAsync();
                }
            }
        }
    }
}
