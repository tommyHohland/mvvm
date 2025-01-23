using System.Windows.Controls;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM.Views.Pages
{
    public partial class EmployeePage : Page
    {
        private readonly EmployeeViewModel _viewModel;
        public EmployeePage()
        {
            InitializeComponent();
            _viewModel = new EmployeeViewModel(new EmployeeService(new Context()));
            DataContext = _viewModel;
        }
    }
}
