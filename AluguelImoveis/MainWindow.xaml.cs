using System.Windows;

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
            var view = new Views.LocatariosView();
            view.Show();
        }

        private void ListarImoveis_Click(object sender, RoutedEventArgs e)
        {
            var view = new Views.ImoveisView();
            view.Show();
        }
    }
}
