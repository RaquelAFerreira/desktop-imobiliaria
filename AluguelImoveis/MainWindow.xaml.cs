using AluguelImoveis.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis
{
    public partial class MainWindow : Window
    {
        private readonly IImovelHttpService _imovelService;
        private readonly ILocatarioHttpService _locatarioService;
        private readonly IAluguelHttpService _aluguelService;


        public MainWindow(
            IImovelHttpService imovelService, ILocatarioHttpService locatarioService, IAluguelHttpService aluguelService)
        {
            InitializeComponent();
            _imovelService = imovelService;
            _locatarioService = locatarioService;
            _aluguelService = aluguelService;

        }
        private void Locatarios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.LocatariosView(_locatarioService));
        }

        private void Imoveis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.ImoveisView(_imovelService));
        }

        private void Alugueis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.AlugueisView(_aluguelService, _imovelService, _locatarioService));
        }
    }
}
