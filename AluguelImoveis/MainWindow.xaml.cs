using AluguelImoveis.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis
{
    public partial class MainWindow : Window
    {
        private readonly IImovelHttpService _imovelService;
        private readonly ILocatarioHttpService _locatarioService;
        //private readonly IAluguellHttpService _aluguelService;


        public MainWindow(
            IImovelHttpService imovelService, ILocatarioHttpService locatarioService)
        {
            InitializeComponent();
            _imovelService = imovelService;
            _locatarioService = locatarioService;
        }
        private void ListarLocatarios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.LocatariosView(_locatarioService));
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
