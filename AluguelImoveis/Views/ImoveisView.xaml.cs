using System.Windows;
using AluguelImoveis.Services;
using System.Threading.Tasks;

namespace AluguelImoveis.Views
{
    public partial class ImoveisView : Window
    {
        public ImoveisView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var imoveis = await ApiService.GetImoveisAsync();
            Dispatcher.Invoke(() =>
            {
                ImoveisList.ItemsSource = imoveis;
            });
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CriarImovelView();
            form.ShowDialog();
            _ = LoadDataAsync();
        }
    }
}
