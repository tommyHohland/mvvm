using System.Windows;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM.Views
{

    public partial class ProjectView : Window
    {
        private ProjectViewModel _viewModel;
        public ProjectView()
        {
            InitializeComponent();
            _viewModel = new ProjectViewModel(new ProjectService(new Context()));
            DataContext = _viewModel;
        }
    }
}
