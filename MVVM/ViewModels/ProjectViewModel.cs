using System.Collections.ObjectModel;
using MVVM.Models;
using MVVM.Services;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MVVM.Views;

namespace MVVM.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {
        private readonly ProjectService _projectService;

        public ObservableCollection<Project> Projects { get; private set; } = new ObservableCollection<Project>();
        public ObservableCollection<Employee> Employees { get; private set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Customer> Customers { get; private set; } = new ObservableCollection<Customer>();
        public ObservableCollection<Executor> Executors { get; private set; } = new ObservableCollection<Executor>();

        private Project _selectedProject;
        private Employee _selectedEmployeeForAdd;
        private TeamOfWorker _selectedEmployeeForRemove;

        private int _filterPriority;
        private DateTime _filterStartDateFrom;
        private DateTime _filterStartDateTo;

        private int _previousManagerId;
        private int[] employeesInTeamIDs;

        public ICommand LoadProjectsCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }
        public ICommand OpenWizardCommand { get; }
        public ICommand AddEmployeeToProjectCommand { get; }
        public ICommand RemoveEmployeeFromProjectCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ApplyDatesFilterCommand { get; }
        public ICommand ApplyPriorityFilterCommand { get; }
        public ICommand DisableFiltersCommand { get; }

        public ProjectViewModel(ProjectService projectService)
        {
            _projectService = projectService;

            LoadProjectsCommand = new RelayCommand(async () => await LoadProjectsAsync());
            SaveProjectCommand = new RelayCommand(async () => await SaveProjectAsync(), CanSaveProject);
            DeleteProjectCommand = new RelayCommand(async () => await DeleteProjectAsync(), CanDeleteProject);
            OpenWizardCommand = new RelayCommand(OpenWizard);
            AddEmployeeToProjectCommand = new RelayCommand(AddEmployeeToProject);
            RemoveEmployeeFromProjectCommand = new RelayCommand(RemoveEmployeeFromProject, CanRemoveEmployee);
            ApplyDatesFilterCommand = new RelayCommand(ApplyDatesFilter);
            ApplyPriorityFilterCommand = new RelayCommand(ApplyPriorityFilter);
            DisableFiltersCommand = new RelayCommand(DisableFilters);

            LoadDataAsync();
        }

        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != null && _selectedProject.Manager != null)
                {
                    _previousManagerId = _selectedProject.Manager.ID;
                    employeesInTeamIDs = SelectedProject.TeamsOfWorkers.Select(tm => tm.ID_Employee).ToArray();
                }
                SetProperty(ref _selectedProject, value);
            }
        }

        public Employee SelectedEmployeeForAdd
        {
            get => _selectedEmployeeForAdd;
            set => SetProperty(ref _selectedEmployeeForAdd, value);
        }

        public TeamOfWorker SelectedEmployeeForRemove
        {
            get => _selectedEmployeeForRemove;
            set => SetProperty(ref _selectedEmployeeForRemove, value);
        }

        public int FilterPriority
        {
            get => _filterPriority;
            set => SetProperty(ref _filterPriority, value);
        }

        public DateTime FilterStartDateFrom
        {
            get => _filterStartDateFrom;
            set => SetProperty(ref _filterStartDateFrom, value);
        }

        public DateTime FilterStartDateTo
        {
            get => _filterStartDateTo;
            set => SetProperty(ref _filterStartDateTo, value);
        }

        private async Task LoadDataAsync()
        {
            await LoadProjectsAsync();
            await LoadEmployeesOrderByFullNameAsync();
            await LoadCustomersOrderByCompanyAsync();
            await ExecutorsOrderByCompanyNameAsync();
        }

        private async Task LoadProjectsAsync()
        {
            Projects.Clear();
            var projects = await _projectService.GetAllProjectsAsync();
            foreach (var project in projects)
                Projects.Add(project);
        }

        private async Task LoadEmployeesOrderByFullNameAsync()
        {
            
            Employees.Clear();
            var employees = await _projectService.GetOrderedEntitiesQuery<Employee, string>(e => e.FullName);
            foreach (var employee in employees)
                Employees.Add(employee);
        }

        private async Task LoadCustomersOrderByCompanyAsync()
        {
            Customers.Clear();
            var customers = await _projectService.GetOrderedEntitiesQuery<Customer, string>(e => e.CompanyName);
            foreach (var customer in customers)
                Customers.Add(customer);
        }

        private async Task ExecutorsOrderByCompanyNameAsync()
        {
            Executors.Clear();
            var executors = await _projectService.GetOrderedEntitiesQuery<Executor, string>(e => e.CompanyName);
            foreach (var executor in executors)
                Executors.Add(executor);
        }

        private async Task SaveProjectAsync()
        {
            try
            {
                if (SelectedProject != null)
                {
                    _projectService.isValidData(SelectedProject);

                    if (SelectedProject.Manager.ID != SelectedProject.ID_Manager)
                    {
                        MessageBoxResult result = MessageBox.Show("Remove the previous manager from a team?", "Remove Manager", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            SetProjectReferences();
                            await _projectService.RemoteManagerFromProject(SelectedProject.ID, _previousManagerId, SelectedProject.Manager?.ID ?? 0);
                            var updatedProject = await _projectService.GetProjectByIdAsync(SelectedProject.ID);
                            UpdateProjectInList(updatedProject);
                        }
                        else
                        {
                            SetProjectReferences();
                            await _projectService.ChangeManagerOnProject(SelectedProject.ID, _previousManagerId, SelectedProject.Manager?.ID ?? 0);
                            var updatedProject = await _projectService.GetProjectByIdAsync(SelectedProject.ID);
                            UpdateProjectInList(updatedProject);
                        }
                    }
                    else
                    {
                        SetProjectReferences();
                        var updatedProject = await _projectService.GetProjectByIdAsync(SelectedProject.ID);
                        UpdateProjectInList(updatedProject);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}","Invalid intput data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SetProjectReferences()
        {
            if (SelectedProject.Manager != null)
                SelectedProject.ID_Manager = SelectedProject.Manager.ID;

            if (SelectedProject.Executor != null)
                SelectedProject.ID_executor = SelectedProject.Executor.ID;

            if (SelectedProject.Customer != null)
                SelectedProject.ID_customer = SelectedProject.Customer.ID;
        }

        private bool CanSaveProject() => SelectedProject != null;

        private async Task DeleteProjectAsync()
        {
            if (SelectedProject != null)
            {
                await _projectService.DeleteProjectAsync(SelectedProject.ID);
                await LoadProjectsAsync();
            }
        }

        private bool CanDeleteProject() => SelectedProject != null && SelectedProject.ID > 0;

        private async void AddEmployeeToProject()
        {
            if (SelectedProject != null && SelectedEmployeeForAdd != null)
            {
                if (employeesInTeamIDs.Contains(SelectedEmployeeForAdd.ID))
                {
                    MessageBox.Show("The employee is already a member of the team", "Employee Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _projectService.AddEmployeeToProjectAsync(SelectedProject.ID, SelectedEmployeeForAdd.ID);
                var updatedProject = await _projectService.GetProjectByIdAsync(SelectedProject.ID);
                UpdateProjectInList(updatedProject);
            }
        }

        private void UpdateProjectInList(Project updatedProject)
        {
            int index = Projects.IndexOf(SelectedProject);
            if (index >= 0)
            {
                Projects[index] = updatedProject;
            }
            SelectedProject = updatedProject;
        }

        private bool CanRemoveEmployee() => SelectedEmployeeForRemove != null;

        private async void RemoveEmployeeFromProject()
        {
            if (SelectedProject != null && SelectedEmployeeForRemove != null)
            {
                if (SelectedEmployeeForRemove.ID_Employee == SelectedProject.ID_Manager)
                {
                    MessageBox.Show("Employee is a manager!\nChange manager", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _projectService.RemoveEmployeeFromProjectAsync(SelectedProject.ID, SelectedEmployeeForRemove.ID_Employee);
                var updatedProject = await _projectService.GetProjectByIdAsync(SelectedProject.ID);
                UpdateProjectInList(updatedProject);
            }
        }

        private void OpenWizard()
        {
            var wizard = new Wizard();
            wizard.ShowDialog();
        }

        private void ApplyDatesFilter()
        {
            if (FilterStartDateFrom != null && FilterStartDateTo != null)
            {
                var filteredProjects = Projects.Where(p => p.DateStart >= FilterStartDateFrom && p.DateStart <= FilterStartDateTo).ToList();

                Projects.Clear();
                foreach (var project in filteredProjects)
                {
                    Projects.Add(project);
                }
            }
            else
            {
                MessageBox.Show("Choose both dates", "Missing Date", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void ApplyPriorityFilter()
        {
            if (FilterPriority != null)
            {
                var filteredProjects = Projects.Where(p => p.Priority == FilterPriority).ToList();

                Projects.Clear();
                foreach (var project in filteredProjects)
                {
                    Projects.Add(project);
                }
            }
            else
            {
                MessageBox.Show("Please enter a priority", "Missing Priority", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DisableFilters()
        {
            FilterPriority = 0;
            FilterStartDateFrom = DateTime.Now;
            FilterStartDateTo = DateTime.Now;
            LoadProjectsAsync();
        }
    }
}
