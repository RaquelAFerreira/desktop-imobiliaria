using System.Windows;
using AluguelImoveis.Services;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class ImoveisView : Page
    {
        public ImoveisView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var imoveis = await ApiService.GetImoveisAsync();
            ImoveisList.ItemsSource = imoveis;
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CriarImovelView();
            form.ShowDialog();
            _ = LoadDataAsync();
        }
    }
}
