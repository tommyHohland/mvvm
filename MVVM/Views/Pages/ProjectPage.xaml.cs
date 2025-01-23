using System.Windows.Controls;
using MVVM.Data;
using MVVM.Services;
using MVVM.ViewModels;

namespace MVVM.Views.Pages
{
   
    public partial class ProjectPage : Page
    {
        private readonly ProjectViewModel _viewModel;
        public ProjectPage()
        {
            InitializeComponent();
            _viewModel = new ProjectViewModel(new ProjectService(new Context()));
            DataContext = _viewModel;
        }
    }
}
