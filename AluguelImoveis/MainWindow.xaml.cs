using AluguelImoveis.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            SetActiveMenuButton(BtnLocatarios);
        }

        private void Imoveis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.ImoveisView(_imovelService));
            SetActiveMenuButton(BtnImoveis);
        }

        private void Alugueis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.AlugueisView(_aluguelService, _imovelService, _locatarioService));
            SetActiveMenuButton(BtnAlugueis);
        }

        private void SetActiveMenuButton(Button activeButton)
        {
            // Lista de todos os botões
            var buttons = new[] { BtnLocatarios, BtnImoveis, BtnAlugueis };

            foreach (var btn in buttons)
            {
                if (btn == activeButton)
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
                    btn.Foreground = Brushes.White;
                    btn.FontWeight = FontWeights.Bold;
                }
                else
                {
                    btn.Background = Brushes.White;
                    btn.Foreground = Brushes.Black;
                    btn.FontWeight = FontWeights.Normal;
                }
            }
        }
    }
}
