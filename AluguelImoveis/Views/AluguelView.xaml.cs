using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services;
using System.Threading.Tasks;
using System.Windows;

namespace AluguelImoveis.Views
{
    public partial class AlugueisView : Window
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
    }
}
