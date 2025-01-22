using System.Text.RegularExpressions;
using MVVM.Data;
using MVVM.Models;

namespace MVVM.Services
{
    public  class EmployeeService
    {
        private readonly Context _context;
        public EmployeeService(Context context)
        {
            _context = context;
        }
        public bool EmployeeIsManager(Employee employee) 
        { 
            return _context.Projects.Any(e => e.ID_Manager == employee.ID) == true; 
        }

        public bool EmployeeIsTeamMember(Employee employee) 
        {
            return _context.TeamsOfWorkers.Any(e => e.ID_Employee == employee.ID) == true;
        }
        private bool isValidData(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FullName))
                throw new ArgumentException("FullName is null or empty");

            if (string.IsNullOrEmpty(employee.Email))
                throw new ArgumentException("Email is null or empty");

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // pattern for sometext@sometext.sometext
            if (!Regex.IsMatch(employee.Email, emailPattern))
                throw new ArgumentException("Email is not in a valid format");

            return true;
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            if (isValidData(employee))
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if (isValidData(employee))
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        public async Task CascadeDeleteEmployeeAsync(Employee employee)
        {
            var teams = _context.TeamsOfWorkers.Where(tw => tw.ID_Employee == employee.ID).ToList();
            foreach (var row in teams)
            {
                _context.TeamsOfWorkers.Remove(row);
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
           
    }
}
