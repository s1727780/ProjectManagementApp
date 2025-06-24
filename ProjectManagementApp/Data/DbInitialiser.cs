using ProjectManagementApp.Model;
using Task = ProjectManagementApp.Model.Task;

namespace ProjectManagementApp.Data {
    public static class DbInitialiser {
        public static void Initialise(TaskContext context) {
            context.Database.EnsureCreated();

            if (context.Projects.Any() || context.Tasks.Any()) {
                return;
            }

            Project[] projects = {
                new Project{Name="P01", Description="Test project 1" },
                new Project{Name="P02", Description="Test project 2" }
            };

            foreach (var project in projects) { 
                context.Projects.Add(project);
            }
            context.SaveChanges();

            Task[] tasks = {
                new Task{Name="T10", DueDate = new DateTime(2026, 1, 1), IsCompleted = false, ProjectId = 1},
                new Task{Name="T20", DueDate = new DateTime(2026, 2, 1), IsCompleted = false, ProjectId = 2},
                new Task{Name="T21", DueDate = new DateTime(2026, 2, 2), IsCompleted = false, ProjectId = 2},
                new Task{Name="T11", DueDate = new DateTime(2026, 1, 2), IsCompleted = false, ProjectId = 1}
            };

            foreach (var task in tasks) { 
                context.Tasks.Add(task);
            }

            context.SaveChanges();

        }

        public static void CreateDbIfNotExists(IHost host) {
            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<TaskContext>();
                    Initialise(context);
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

    }
}
