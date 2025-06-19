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

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }
    }

    interface IProjectService
    {
        Project? GetProjectById(int id);
        List<Project> GetProjects();
        void DeleteProjectById(int id);
        Project AddProject(Project project);
    }
}
