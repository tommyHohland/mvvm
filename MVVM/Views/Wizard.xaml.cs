using System.Windows;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM.Views
{ 
    public partial class Wizard : Window
    {
        private WizardViewModel _viewModel;
        public Wizard()
        {
            InitializeComponent();
            _viewModel = new WizardViewModel(new ProjectService(new Context()));
            DataContext = _viewModel;
        }
    }
}
