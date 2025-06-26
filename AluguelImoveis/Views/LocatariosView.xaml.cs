using System.Windows;
using AluguelImoveis.Services;
using System.Threading.Tasks;

namespace AluguelImoveis.Views
{
    public partial class LocatariosView : Window
    {
        public LocatariosView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var locatarios = await ApiService.GetLocatariosAsync();
            LocatariosList.ItemsSource = locatarios;
        }
        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new CriarLocatarioView();
            form.ShowDialog();
            _ = LoadDataAsync(); // Recarrega a lista
        }
    }
}
