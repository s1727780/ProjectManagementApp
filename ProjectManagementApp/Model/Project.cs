using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model
{
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    interface IProjectService
    {
        Project? GetProjectById(int id);
        List<Project> GetProjects();
        void DeleteProjectById(int id);
        Project AddProject(Project project);
    }
}
