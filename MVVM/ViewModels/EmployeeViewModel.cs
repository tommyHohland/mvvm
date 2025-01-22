using System.Collections.ObjectModel;
using MVVM.Models;
using MVVM.Services;
using System.Windows.Input;
using System.Windows;

namespace MVVM.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private readonly EmployeeService _employeeService;
        public ObservableCollection<Employee> Employees { get; private set; } = new ObservableCollection<Employee>();
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public ICommand LoadEmployeesCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        public EmployeeViewModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;

            LoadEmployeesCommand = new RelayCommand(async () => await LoadEmployeesAsync());
            SaveChangesCommand = new RelayCommand(async () => await SaveChangesAsync());
            DeleteEmployeeCommand = new RelayCommand(async () => await DeleteEmployeeAsync());

            LoadEmployeesCommand.Execute(null);
        }

        public async Task LoadEmployeesAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                if (SelectedEmployee == null)
                {
                    MessageBox.Show("No employee selected to save changes.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedEmployee.ID == 0)
                {
                    await _employeeService.AddEmployeeAsync(SelectedEmployee);
                }
                else
                {
                    await _employeeService.UpdateEmployeeAsync(SelectedEmployee);
                }

                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadEmployeesAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task DeleteEmployeeAsync()
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("No employee selected for deletion.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_employeeService.EmployeeIsManager(SelectedEmployee))
                {
                    MessageBox.Show("Employee is a Manager! Please remove them from the manager position first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_employeeService.EmployeeIsTeamMember(SelectedEmployee))
                {
                    var result = MessageBox.Show(
                        "Employee is currently on a team. Remove them from the team?",
                        "Information",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        await _employeeService.CascadeDeleteEmployeeAsync(SelectedEmployee);
                        Employees.Remove(SelectedEmployee);
                        SelectedEmployee = null;
                        await LoadEmployeesAsync();
                    }
                    return;
                }

                await _employeeService.DeleteEmployeeAsync(SelectedEmployee);

                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
                await LoadEmployeesAsync();

                MessageBox.Show("Selected employee deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SelectedEmployee = null;
            }
        }
    }
}
