using AluguelImoveis.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis
{
    public partial class MainWindow : Window
    {
        private readonly IImovelHttpService _imovelService;

        public MainWindow(
            IImovelHttpService imovelService)
        {
            InitializeComponent();
            _imovelService = imovelService;

        }
        private void ListarLocatarios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.LocatariosView()); // <-- Page
        }

        private void ListarImoveis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.ImoveisView(_imovelService));
        }

        private void ListarAlugueis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.AlugueisView());
        }
    }
}
