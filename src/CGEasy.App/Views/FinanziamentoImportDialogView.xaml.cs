using System.Windows;
using CGEasy.App.ViewModels;

namespace CGEasy.App.Views;

public partial class FinanziamentoImportDialogView : Window
{
    public FinanziamentoImportDialogView()
    {
        InitializeComponent();
    }

    public FinanziamentoImportDialogView(FinanziamentoImportDialogViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.RequestClose += (s, e) =>
        {
            DialogResult = viewModel.DialogResult;
            Close();
        };
    }
}

