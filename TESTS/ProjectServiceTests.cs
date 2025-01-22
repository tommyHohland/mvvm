using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using MVVM.Data;
using MVVM.Models;
using MVVM.Services;

namespace TESTS
{
    [TestClass]
    public class ProjectServiceTests
    {
        private DbContextOptions<Context> _options;
        private ProjectService _projectService;
        private int ID_PROJECT;
        private int ID_EXECUTOR;
        private int ID_EMPLOYEE;
        private int ID_CUSTOMER;
        public ProjectServiceTests()
        {
            _options = new DbContextOptionsBuilder<Context>()
                        .UseInMemoryDatabase(databaseName: "TestDatabase")
                        .Options;
        }
        private Context GetContext()
        {
            var context = new Context(_options);
            context.Database.EnsureCreated();
            _projectService = new ProjectService(context);
            return context;
        }

        private void dummydump()
        {
            using (var context = GetContext())
            {
                var executor = new Executor() { CompanyName = "Test" };
                var customer = new Customer() { CompanyName = "Test" };
                var emp = new Employee() { FullName = "Test", Email = "test@gmail.com" };
                var project = new Project()
                {
                    Title = "Test",
                    ID_customer = customer.ID,
                    ID_Manager = emp.ID,
                    ID_executor = executor.ID,
                    DateEnd = DateTime.UtcNow,
                    DateStart = DateTime.UtcNow.AddDays(5),
                    Priority = 1000
                };
                context.Customers.Add(customer);
                context.Employees.Add(emp);
                context.Executors.Add(executor);
                context.Projects.Add(project);

                var tm = new TeamOfWorker() { ID_Employee = emp.ID, ID_Project = project.ID };
                context.TeamsOfWorkers.Add(tm);
                context.SaveChanges();
                ID_PROJECT = project.ID;
                ID_EXECUTOR = executor.ID;
                ID_EMPLOYEE = emp.ID;
                ID_CUSTOMER = customer.ID;
            }
        }

        [TestMethod]
        public async Task IsValidData_ShouldThrowEx3times()
        {
            using (var context = GetContext())
            {
                var executor = new Executor() { CompanyName = "Test" };
                var customer = new Customer() { CompanyName = "Test" };
                var emp = new Employee() { FullName = "Test", Email = "test@gmail.com" };
                context.Customers.Add(customer);
                context.Employees.Add(emp);
                context.Executors.Add(executor);
                context.SaveChanges();

                var invalidTitle = new Project()
                {
                    ID_customer = customer.ID,
                    ID_Manager = emp.ID,
                    ID_executor = executor.ID,
                    DateEnd = DateTime.UtcNow,
                    DateStart = DateTime.UtcNow.AddDays(5),
                    Priority = 1
                };
                var invalidPriority = new Project()
                {
                    Title = "Test",
                    ID_customer = customer.ID,
                    ID_Manager = emp.ID,
                    ID_executor = executor.ID,
                    DateEnd = DateTime.UtcNow.AddDays(5),
                    DateStart = DateTime.UtcNow,
                    Priority = 1000
                };
                var invalidDateRange = new Project()
                {
                    Title = "Test",
                    ID_customer = customer.ID,
                    ID_Manager = emp.ID,
                    ID_executor = executor.ID,
                    DateEnd = DateTime.UtcNow,
                    DateStart = DateTime.UtcNow.AddDays(50),
                    Priority = 1
                };
                var invalidExecutor = new Project()
                {
                    Title = "Test",
                    ID_customer = customer.ID,
                    ID_Manager = emp.ID,
                    DateEnd = DateTime.UtcNow.AddDays(5),
                    DateStart = DateTime.UtcNow,
                    Priority = 1
                };
                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _projectService.AddProject(invalidTitle));
                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _projectService.AddProject(invalidPriority));
                await Assert.ThrowsExceptionAsync<ArgumentException>(() => _projectService.AddProject(invalidDateRange));
            }
        }

        [TestMethod]

        public async Task GetProjectByIdAsync_ShouldReturnProject()
        {
            dummydump();
            Assert.IsTrue(_projectService.GetProjectByIdAsync(ID_PROJECT) != null);
        }

        [TestMethod]
        public async Task DeleteProjectAsync_ShouldDeleteProject()
        {
            dummydump();
            using (var context = GetContext())
            {
                await _projectService.DeleteProjectAsync(ID_PROJECT);

                var deletedProject = await context.Projects.FirstOrDefaultAsync(p => p.ID == ID_PROJECT);
                Assert.IsNull(deletedProject);
            }
        }

        [TestMethod]
        public async Task AddEmployeeToProjectAsync_ShouldAddEmployeeToProject()
        {
            dummydump();
            using (var context = GetContext())
            {
                
                var employee = new Employee { FullName = "Test Employee", Email = "test@example.com" };
                context.Employees.Add(employee);
                context.SaveChanges();


                await _projectService.AddEmployeeToProjectAsync(ID_PROJECT, employee.ID);

                var team = await context.TeamsOfWorkers
                    .FirstOrDefaultAsync(t => t.ID_Project == ID_PROJECT && t.ID_Employee == employee.ID);
                Assert.IsNotNull(team);
            }
        }
    }
}