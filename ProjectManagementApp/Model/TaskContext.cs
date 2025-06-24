using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model {
    public class TaskContext : DbContext {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Task>().ToTable("Task");
            modelBuilder.Entity<Project>().ToTable("Project");
        }
    }
}
