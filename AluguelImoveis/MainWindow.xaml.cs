using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListarLocatarios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.LocatariosView()); // <-- Page
        }

        private void ListarImoveis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.ImoveisView());
        }

        private void ListarAlugueis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.AlugueisView());
        }
    }
}
