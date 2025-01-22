using System.Windows;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        private readonly EmployeeViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new EmployeeViewModel(new EmployeeService(new Context()));
            DataContext = _viewModel;
        }
    }
}