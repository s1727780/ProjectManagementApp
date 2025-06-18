using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model
{
    public class ProjectContext : DbContext {
        public DbSet<Task> Task { get; set; }

        public ProjectContext(DbContextOptions options) : base(options) { 
        
        }
    }
}
