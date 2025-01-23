using System.Windows;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        private readonly NavigationViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new NavigationViewModel(MainFrame);
            DataContext = _viewModel;
        }
    }
}