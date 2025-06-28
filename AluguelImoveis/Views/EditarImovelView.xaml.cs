using AluguelImoveis.Models;
using AluguelImoveis.Services;
using System.Windows;
using System.Windows.Controls;

namespace AluguelImoveis.Views
{
    public partial class EditarImovelView : Window
    {
        private Imovel _imovel;

        public EditarImovelView(Imovel imovel)
        {
            InitializeComponent();
            _imovel = imovel;
            PreencherCampos();
        }

        private void PreencherCampos()
        {
            CodigoBox.Text = _imovel.Codigo;
            EnderecoBox.Text = _imovel.Endereco;
            ValorBox.Text = _imovel.ValorLocacao.ToString();
            DisponivelCheck.IsChecked = _imovel.Disponivel;

            // Seleciona o item do ComboBox com o mesmo texto do tipo atual
            TipoComboBox.SelectedItem = TipoComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content?.ToString() == _imovel.Tipo);
        }

        private async void Salvar_Click(object sender, RoutedEventArgs e)
        {
            _imovel.Codigo = CodigoBox.Text;
            _imovel.Endereco = EnderecoBox.Text;
            _imovel.Tipo = (TipoComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            _imovel.Disponivel = DisponivelCheck.IsChecked == true;
            decimal.TryParse(ValorBox.Text, out decimal valor);
            _imovel.ValorLocacao = valor;

            try
            {
                await ApiService.AtualizarImovelAsync(_imovel);
                MessageBox.Show("Imóvel atualizado com sucesso!");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar imóvel: {ex.Message}");
            }
        }
    }
}
