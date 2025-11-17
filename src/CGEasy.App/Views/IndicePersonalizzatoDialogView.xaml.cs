using System.Windows;
using CGEasy.App.ViewModels;

namespace CGEasy.App.Views
{
    public partial class IndicePersonalizzatoDialogView : Window
    {
        public IndicePersonalizzatoDialogView()
        {
            InitializeComponent();
            
            // Passa il riferimento alla window al ViewModel quando viene impostato il DataContext
            Loaded += (s, e) =>
            {
                if (DataContext is IndicePersonalizzatoDialogViewModel viewModel)
                {
                    viewModel.SetOwner(this);
                }
            };
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is IndicePersonalizzatoDialogViewModel viewModel)
            {
                if (viewModel.DialogResult)
                {
                    DialogResult = true;
                }
            }
            base.OnClosing(e);
        }
    }
}

