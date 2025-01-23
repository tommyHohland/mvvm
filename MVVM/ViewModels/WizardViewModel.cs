using System.Collections.ObjectModel;
using MVVM.Models;
using MVVM.Services;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace MVVM.ViewModels
{
    public class WizardViewModel : BaseViewModel
    {
        #region Fields
        private readonly ProjectService _projectService;
        private Project _projectToAdd = new Project();
        private Employee _selectedEmployee = new Employee();

        private bool _isStep1Valid;
        private bool _isStep2Valid;
        private bool _isStep3Valid;
        private bool _isStep4Valid;
        private bool _isNextButtonEnabled = false;

        private int _currentStep = 1;
        private string _textForSearchCustomer;
        private string _textForSearchExecutor;
        private string _textForSearchManager;

        private Visibility _isStep1Visible = Visibility.Visible;
        private Visibility _isStep2Visible = Visibility.Collapsed;
        private Visibility _isStep3Visible = Visibility.Collapsed;
        private Visibility _isStep4Visible = Visibility.Collapsed;
        #endregion

        #region Properties
        public ObservableCollection<Employee> Employees { get; set; } = new();
        public ObservableCollection<Customer> Customers { get; set; } = new();
        public ObservableCollection<Executor> Executors { get; set; } = new();
        public ObservableCollection<Employee> SelectedEmployees { get; set; } = new();

        public string ProjectTitle
        {
            get => _projectToAdd.Title;
            set
            {
                _projectToAdd.Title = value;
                OnPropertyChanged(nameof(ProjectTitle));
                ValidateStep1();
            }
        }

        public DateTime? DateStart { get; set; } = DateTime.Now;
        public DateTime? DateEnd { get; set; } = DateTime.Now.AddDays(7);

        public int Priority
        {
            get => _projectToAdd.Priority;
            set
            {
                _projectToAdd.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public Customer SelectedCustomer
        {
            get => _projectToAdd.Customer;
            set
            {
                _projectToAdd.Customer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                ValidateStep2();
            }
        }

        public Executor SelectedExecutor
        {
            get => _projectToAdd.Executor;
            set
            {
                _projectToAdd.Executor = value;
                OnPropertyChanged(nameof(SelectedExecutor));
                ValidateStep2();
            }
        }

        public Employee SelectedManager
        {
            get => _projectToAdd.Manager;
            set
            {
                _projectToAdd.Manager = value;
                OnPropertyChanged(nameof(SelectedManager));
                SelectedEmployees.Add(SelectedManager);
                ValidateStep3();
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public string TextForSearchCustomer
        {
            get => _textForSearchCustomer;
            set
            {
                if (_textForSearchCustomer != value)
                {
                    _textForSearchCustomer = value;
                    OnPropertyChanged(nameof(TextForSearchCustomer));
                    _projectService.FilterCollection(Customers, _textForSearchCustomer, customer => customer.CompanyName);
                }
            }
        }

        public string TextForSearchExecutor
        {
            get => _textForSearchExecutor;
            set
            {
                if (_textForSearchExecutor != value)
                {
                    _textForSearchExecutor = value;
                    OnPropertyChanged(nameof(TextForSearchExecutor));
                    _projectService.FilterCollection(Executors, _textForSearchExecutor, executor => executor.CompanyName);
                }
            }
        }

        public string TextForSearchManager
        {
            get => _textForSearchManager;
            set
            {
                if (_textForSearchManager != value)
                {
                    _textForSearchManager = value;
                    OnPropertyChanged(nameof(TextForSearchManager));
                    _projectService.FilterCollection(Employees, _textForSearchManager, manager => manager.FullName);
                }
            }
        }

        #region Visibility Steps
        public Visibility IsStep1Visible
        {
            get => _isStep1Visible;
            set
            {
                _isStep1Visible = value;
                OnPropertyChanged(nameof(IsStep1Visible));
            }
        }

        public Visibility IsStep2Visible
        {
            get => _isStep2Visible;
            set
            {
                _isStep2Visible = value;
                OnPropertyChanged(nameof(IsStep2Visible));
            }
        }

        public Visibility IsStep3Visible
        {
            get => _isStep3Visible;
            set
            {
                _isStep3Visible = value;
                OnPropertyChanged(nameof(IsStep3Visible));
            }
        }

        public Visibility IsStep4Visible
        {
            get => _isStep4Visible;
            set
            {
                _isStep4Visible = value;
                OnPropertyChanged(nameof(IsStep4Visible));
            }
        }

        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
                UpdateStepVisibility();
            }
        }

        public bool IsStep1Valid
        {
            get => _isStep1Valid;
            set
            {
                _isStep1Valid = value;
                OnPropertyChanged(nameof(IsStep1Valid));
                UpdateNextButtonState();
            }
        }

        public bool IsStep2Valid
        {
            get => _isStep2Valid;
            set
            {
                _isStep2Valid = value;
                OnPropertyChanged(nameof(IsStep2Valid));
                UpdateNextButtonState();
            }
        }

        public bool IsStep3Valid
        {
            get => _isStep3Valid;
            set
            {
                _isStep3Valid = value;
                OnPropertyChanged(nameof(IsStep3Valid));
                UpdateNextButtonState();
            }
        }

        public bool IsStep4Valid
        {
            get => _isStep4Valid;
            set
            {
                _isStep4Valid = value;
                OnPropertyChanged(nameof(IsStep4Valid));
                UpdateNextButtonState();
            }
        }

        public bool IsNextButtonEnabled
        {
            get => _isNextButtonEnabled;
            set
            {
                _isNextButtonEnabled = value;
                OnPropertyChanged(nameof(IsNextButtonEnabled));
            }
        }
        #endregion

        #region Commands
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand FinishCommand { get; }
        public ICommand JoinEmployeeCommand { get; }
        public ICommand RemoteEmployeeCommand { get; }
        #endregion

        #endregion

        #region Methods

        private void ResetFilters()
        {
            CollectionViewSource.GetDefaultView(Customers).Filter = null;
            CollectionViewSource.GetDefaultView(Executors).Filter = null;
            CollectionViewSource.GetDefaultView(Employees).Filter = null;
        }

        private void UpdateStepVisibility()
        {
            IsStep1Visible = CurrentStep == 1 ? Visibility.Visible : Visibility.Collapsed;
            IsStep2Visible = CurrentStep == 2 ? Visibility.Visible : Visibility.Collapsed;
            IsStep3Visible = CurrentStep == 3 ? Visibility.Visible : Visibility.Collapsed;
            IsStep4Visible = CurrentStep == 4 ? Visibility.Visible : Visibility.Collapsed;

            if (CurrentStep == 4)
            {
                ResetFilters();
            }

            UpdateNextButtonState();
        }

        private void UpdateNextButtonState()
        {
            switch (CurrentStep)
            {
                case 1:
                    IsNextButtonEnabled = IsStep1Valid;
                    break;
                case 2:
                    IsNextButtonEnabled = IsStep2Valid;
                    break;
                case 3:
                    IsNextButtonEnabled = IsStep3Valid;
                    break;
                case 4:
                    IsNextButtonEnabled = IsStep4Valid;
                    break;
            }
        }

        private void ValidateStep1()
        {
            IsStep1Valid = !string.IsNullOrWhiteSpace(ProjectTitle)
                                                        && DateStart.HasValue
                                                        && DateEnd.HasValue
                                                        && int.TryParse(Priority.ToString(), out _);
        }

        private void ValidateStep2()
        {
            IsStep2Valid = SelectedCustomer != null && SelectedExecutor != null;
        }

        private void ValidateStep3()
        {
            IsStep3Valid = SelectedManager != null;
        }

        private void ValidateStep4()
        {
            IsStep4Valid = SelectedEmployees.Any();
        }

        private void NextStep()
        {
            if (CurrentStep < 4)
                CurrentStep++;
        }

        private void PreviousStep()
        {
            if (CurrentStep > 1)
                CurrentStep--;
        }

        private async void Finish()
        {
            if (DateStart == null || DateEnd == null)
            {
                MessageBox.Show("Start and End dates must be selected.");
                return;
            }

            _projectToAdd.DateStart = DateStart.Value;
            _projectToAdd.DateEnd = DateEnd.Value;

            await _projectService.AddProjectAsync(_projectToAdd);
            MessageBox.Show($"Project '{_projectToAdd.Title}' created successfully!");

            var employeeIds = SelectedEmployees.Select(e => e.ID).ToArray();

            if (employeeIds.Length > 0)
            {
                await _projectService.AddEmployeesToProjectAsync(_projectToAdd.ID, employeeIds);
                MessageBox.Show("Employees have been added to the project.");
            }
            else
            {
                MessageBox.Show("No employees selected.");
            }

            SelectedEmployees.Clear();
            _projectToAdd = null;
            CurrentStep = 1;
        }

        private void JoinEmployee()
        {
            if (_selectedEmployee != null)
            {
                if (SelectedEmployees.Contains(SelectedEmployee))
                {
                    MessageBox.Show("Employee is already on the team list");
                    return;
                }
                SelectedEmployees.Add(SelectedEmployee);
                ValidateStep4();
            }
            else
                MessageBox.Show("No employee selected.");
        }

        private void RemoteEmployee()
        {
            if (_selectedEmployee != null)
            {
                if (SelectedEmployee != SelectedManager)
                    SelectedEmployees.Remove(SelectedEmployee);
                else
                    MessageBox.Show("Employee is a manager. Change the manager before removing.");
                ValidateStep4();
            }
            else
                MessageBox.Show("No employee selected.");
        }

        private async Task LoadDataAsync()
        {
            await LoadEmployeesAsync();
            await LoadCustomersAsync();
            await LoadExecutorsAsync();
        }

        private async Task LoadEmployeesAsync()
        {
            Employees.Clear();
            var employees = await _projectService.GetOrderedEntitiesQuery<Employee, string> (e => e.FullName);
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private async Task LoadCustomersAsync()
        {
            Customers.Clear();
            var customers = await _projectService.GetOrderedEntitiesQuery<Customer, string> (c => c.CompanyName);
            foreach (var customer in customers)
                Customers.Add(customer);
        }

        private async Task LoadExecutorsAsync()
        {
            Executors.Clear();
            var executors = await _projectService.GetOrderedEntitiesQuery<Executor, string>(c => c.CompanyName);
            foreach (var executor in executors)
                Executors.Add(executor);
        }
        #endregion

        #region Constructor
        public WizardViewModel(ProjectService projectService)
        {
            _projectService = projectService;
            NextCommand = new RelayCommand(NextStep);
            PreviousCommand = new RelayCommand(PreviousStep);
            FinishCommand = new RelayCommand(Finish);
            JoinEmployeeCommand = new RelayCommand(JoinEmployee);
            RemoteEmployeeCommand = new RelayCommand(RemoteEmployee);
            LoadDataAsync();
        }
        #endregion
    }
}
