using ProjectManagementApp.Model;
using Task = ProjectManagementApp.Model.Task;

namespace ProjectManagementApp.Data {
    public static class DbInitialiser {
        public static void Initialise(TaskContext context) {
            context.Database.EnsureCreated();

            if (context.Projects.Any() || context.Tasks.Any()) {
                return;
            }

            Project[] projects = new Project[] {
                new Project{ }
            };

            foreach (var project in projects) { 
                context.Projects.Add(project);
            }
            context.SaveChanges();

            Task[] tasks = new Task[] {
                new Task{ }
            };

            foreach (var task in tasks) { 
                context.Tasks.Add(task);
            }

            context.SaveChanges();

        }

    }
}
