﻿using MVVM.Models;
using System.Windows.Data;
using MVVM.Data;
using Microsoft.EntityFrameworkCore;

namespace MVVM.Services
{
    public  class ProjectService
    {
        private readonly Context _dbContext;

        public ProjectService(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Executor)
                .Include(p => p.Customer)
                .Include(p => p.TeamsOfWorkers)
                    .ThenInclude(t => t.Employee)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Executor)
                .Include(p => p.Customer)
                .Include(p => p.TeamsOfWorkers)
                    .ThenInclude(t => t.Employee)
                .FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task UpdateProjectAsync(Project project)
        {

            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<List<T>> GetOrderedEntitiesQuery<T, TKey>(Func<T, TKey> keySelector) where T : class
        {
            return Task.FromResult(_dbContext.Set<T>().OrderBy(keySelector).ToList());
        }

        public async Task AddProjectAsync(Project project)
        {
            if (isValidData(project))
            {
                _dbContext.Projects.Add(project);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddEmployeeToProjectAsync(int projectId, int employeeId)
        {
            var project = await _dbContext.Projects.Include(p => p.TeamsOfWorkers).FirstOrDefaultAsync(p => p.ID == projectId);
            var employee = await _dbContext.Employees.FindAsync(employeeId);

            if (project != null && employee != null)
            {
                project.TeamsOfWorkers.Add(new TeamOfWorker { ID_Project = projectId, ID_Employee = employeeId });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddEmployeesToProjectAsync(int projectId, int[] employeesId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ID == projectId);
            foreach (int id in employeesId)
            {
                project.TeamsOfWorkers.Add(new TeamOfWorker { ID_Project = projectId, ID_Employee = id });
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
        {
            var project = await _dbContext.Projects.Include(p => p.TeamsOfWorkers).FirstOrDefaultAsync(p => p.ID == projectId);
            var team = project?.TeamsOfWorkers.FirstOrDefault(t => t.ID_Employee == employeeId);

            if (team != null)
            {
                project.TeamsOfWorkers.Remove(team);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangeManagerOnProject(int projectId, int previousManagerId, int managerId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ID == projectId);
            if (project != null)
            {
                project.ID_Manager = managerId;

                var managerInTeam = await _dbContext.TeamsOfWorkers
                    .AnyAsync(t => t.ID_Project == projectId && t.ID_Employee == managerId);

                if (!managerInTeam)
                {
                    var team = new TeamOfWorker { ID_Employee = managerId, ID_Project = projectId };
                    _dbContext.TeamsOfWorkers.Add(team);
                }

                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task RemoteManagerFromProject(int projectId, int previousManagerId, int managerId)
        {
            var team = await _dbContext.TeamsOfWorkers.FirstOrDefaultAsync(p => p.ID_Project == projectId
                                                                                && p.ID_Employee == previousManagerId);
            var managerInTeam = await _dbContext.TeamsOfWorkers
                    .AnyAsync(t => t.ID_Project == projectId && t.ID_Employee == managerId);

            if (team != null)
            {
                _dbContext.TeamsOfWorkers.Remove(team);
            }

            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ID == projectId);
            if (project != null)
            {
                project.ID_Manager = managerId;

                if (!managerInTeam)
                {
                    var newTeam = new TeamOfWorker { ID_Employee = managerId, ID_Project = projectId };
                    _dbContext.TeamsOfWorkers.Add(newTeam);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        public bool isValidData(Project project)
        {
            if (string.IsNullOrEmpty(project.Title))
                throw new ArgumentException("Title is null or empty");

            if (project.DateEnd < project.DateStart)
                throw new ArgumentException("DateStart cant be less than DateEnd");

            if (project.Priority < 0 || project.Priority > 10)
                throw new ArgumentException(nameof(project.Priority), "Priority must be between 0 and 10.");

            if (project.ID_executor == null)
                throw new ArgumentException(nameof(project.Priority), "The performing company has not been determined");

            return true;
        }
    }
}
