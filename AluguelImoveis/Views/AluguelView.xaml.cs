using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            List<AluguelDetalhadoDto> alugueis = await ApiService.GetAlugueisAsync();
            AlugueisList.ItemsSource = alugueis;
        }

        private async void FiltrarAtivos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ativos = await ApiService.GetAlugueisAtivosAsync();
                AlugueisList.ItemsSource = ativos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar aluguéis ativos: {ex.Message}");
            }
        }

        private void CadastrarAluguel_Click(object sender, RoutedEventArgs e)
        {
            var view = new CriarAluguelView();
            view.ShowDialog();
            _ = LoadDataAsync(); // Recarrega os dados após fechar
        }
    }
}
