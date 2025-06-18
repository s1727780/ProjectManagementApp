using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model
{
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProjectContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public ProjectContext(DbContextOptions options) : base(options)
        {

        }
    }
}
