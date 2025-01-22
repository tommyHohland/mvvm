using MVVM.Services;
using MVVM.Models;
using MVVM.Data;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace TESTS
{
    [TestClass]
    public sealed class EmployeeServiceTests
    {
        private DbContextOptions<Context> _options;
        private EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _options = new DbContextOptionsBuilder<Context>()
                        .UseInMemoryDatabase(databaseName: "TestDatabase")
                        .Options;
        }

        private Context GetContext()
        {
            var context = new Context(_options);
            context.Database.EnsureCreated();
            _employeeService = new EmployeeService(context);
            return context;
        }

        [TestMethod]
        public async Task IsValidData_ShouldThrowEx3times()
        {
            using (var context = GetContext())
            {
                var emptyEmail = new Employee { FullName = "Ivanov", Email = "" };
                var invalidFormatEmail = new Employee { FullName = "Ivanov", Email = "mail@.com" };
                var emptyFullName = new Employee { FullName = "", Email = "test@gmail.com" };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeService.AddEmployeeAsync(emptyEmail));
                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeService.AddEmployeeAsync(invalidFormatEmail));
                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeService.AddEmployeeAsync(emptyFullName));
            }
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployee()
        {
            using (var context = GetContext())
            {
                var existingEmployee = new Employee { FullName = "Old Name", Email = "old.email@example.com" };
                context.Employees.Add(existingEmployee);
                await context.SaveChangesAsync();

                existingEmployee.FullName = "Updated Name";

                await _employeeService.UpdateEmployeeAsync(existingEmployee);

                var updatedEmployee = await context.Employees.FindAsync(existingEmployee.ID);
                Assert.AreEqual("Updated Name", updatedEmployee.FullName);
            }
        }

        [TestMethod]
        public async Task AddEmployeeAsync_ShouldAddEmployee()
        {
            using (var context = GetContext())
            {
                var newEmployee = new Employee { FullName = "New Emp", Email = "Vjzafvbkbz123@gmail.com" };
                await _employeeService.AddEmployeeAsync(newEmployee);

                var addedEmployee = await context.Employees.FindAsync(newEmployee.ID);
                Assert.AreEqual("Vjzafvbkbz123@gmail.com", addedEmployee.Email);
            }
        }

        [TestMethod]
        public async Task EmployeeIsManager_ShouldReturnTrue()
        {
            using (var context = GetContext())
            {
                var manager = new Employee() { Email = "123@gmail.com", FullName = "True Manager" };
                var executor = new Executor() { CompanyName = "E_Company" };
                var customer = new Customer() { CompanyName = "C_Company" };

                context.Employees.Add(manager);
                context.Customers.Add(customer);
                context.Executors.Add(executor);

                var project = new Project()
                {
                    DateEnd = DateTime.Now.AddDays(1),
                    DateStart = DateTime.Now,
                    ID_customer = customer.ID,
                    ID_executor = executor.ID,
                    ID_Manager = manager.ID,
                    Title = "title",
                    Priority = 1
                };
                var tm_m = new TeamOfWorker() { ID_Employee = manager.ID, ID_Project = project.ID };

                
                context.Projects.Add(project);
                context.TeamsOfWorkers.Add(tm_m);

                context.SaveChangesAsync();
                bool fact = _employeeService.EmployeeIsManager(manager);
                Assert.AreEqual(true, fact);
            }
        }

        [TestMethod]
        public async Task EmployeeIsManager_ShouldReturnFalse()
        {
            using (var context = GetContext())
            {
                var nonmanager = new Employee() { Email = "1234@gmail.com", FullName = "False Manager" };
                context.Employees.Add(nonmanager);
                await context.SaveChangesAsync();
                Assert.AreEqual(false, _employeeService.EmployeeIsManager(nonmanager));
            }
        }

        [TestMethod]
        public async Task EmployeeIsTeamMember_ShouldReturnTrue()
        {
            using (var context = GetContext())
            {
                var manager = new Employee() { Email = "manager@gmail.com", FullName = "manager" };
                var teamMember = new Employee() { Email = "teammember@gmail.com", FullName = "TeamMember" };
                var executor = new Executor() { CompanyName = "E_Company" };
                var customer = new Customer() { CompanyName = "C_Company" };

                var project = new Project()
                {
                    DateEnd = DateTime.Now.AddDays(1),
                    DateStart = DateTime.Now,
                    ID_customer = customer.ID,
                    ID_executor = executor.ID,
                    ID_Manager = manager.ID,
                    Title = "title",
                    Priority = 1
                };

                var tm_m = new TeamOfWorker() { ID_Employee = manager.ID, ID_Project = project.ID };
                var tm_e = new TeamOfWorker() { ID_Employee = teamMember.ID, ID_Project = project.ID };

                context.Employees.Add(manager);
                context.Customers.Add(customer);
                context.Executors.Add(executor);
                context.Projects.Add(project);
                context.TeamsOfWorkers.Add(tm_m);
                context.TeamsOfWorkers.Add(tm_e);

                await context.SaveChangesAsync();
                Assert.AreEqual(true, _employeeService.EmployeeIsTeamMember(teamMember));
            }
        }

        [TestMethod]
        public async Task EmployeeIsTeamMember_ShouldReturnFalse()
        {
            using (var context = GetContext())
            {
                var nonTeamMember = new Employee() { Email = "nonteammember@gmail.com", FullName = "nonTeamMember" };
                context.Employees.Add(nonTeamMember);
                await context.SaveChangesAsync();
                Assert.AreEqual(false, _employeeService.EmployeeIsTeamMember(nonTeamMember));
            }
        }
    }
}
