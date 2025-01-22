using System.Configuration;
using Microsoft.EntityFrameworkCore;
using MVVM.Models;


namespace MVVM.Data
{
    internal class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Executor> Executors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TeamOfWorker> TeamsOfWorkers { get; set; }

        public Context() { }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamOfWorker>()
                .HasOne(t => t.Project)
                .WithMany(p => p.TeamsOfWorkers)
                .HasForeignKey(t => t.ID_Project);

            modelBuilder.Entity<TeamOfWorker>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.TeamsOfWorkers)
                .HasForeignKey(t => t.ID_Employee);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ID_Manager)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
