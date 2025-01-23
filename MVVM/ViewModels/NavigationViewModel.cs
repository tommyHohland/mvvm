using MVVM.Views.Pages;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;

        public ICommand OpenEmployeePageCommand { get; }
        public ICommand OpenProjectPageCommand { get; }
        public ICommand OpenWizardPageCommand { get; }

        public NavigationViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;

            OpenEmployeePageCommand = new RelayCommand(OpenEmployeePage);
            OpenProjectPageCommand = new RelayCommand(OpenProjectPage);
        }

        private void OpenEmployeePage()
        {
            _mainFrame.Navigate(new EmployeePage());
        }

        private void OpenProjectPage()
        {
            _mainFrame.Navigate(new ProjectPage());
        }
    }
}