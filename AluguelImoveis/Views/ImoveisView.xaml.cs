using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class ImoveisView : Page
    {        
        private List<Imovel> todosImoveis = new();

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
            var filtrados = todosImoveis.AsEnumerable();

            if (TipoComboBox.Text is string tipoSelecionado && TipoComboBox.Text != "")
            {
                filtrados = filtrados.Where(i => i.Tipo == tipoSelecionado);
            }

            if (decimal.TryParse(ValorMinBox.Text, out decimal valMin))
            {
                filtrados = filtrados.Where(i => i.ValorLocacao >= valMin);
            }

            if (decimal.TryParse(ValorMaxBox.Text, out decimal valMax))
            {
                filtrados = filtrados.Where(i => i.ValorLocacao <= valMax);
            }

            ImoveisList.ItemsSource = filtrados.ToList();
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
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir imóvel: {ex.Message}");
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
