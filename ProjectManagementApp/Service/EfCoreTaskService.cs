using ProjectManagementApp.Model;
using Task = ProjectManagementApp.Model.Task;

namespace ProjectManagementApp.Service {
    public class EfCoreTaskService : ITaskService {
        private readonly TaskContext _context;

        public EfCoreTaskService(TaskContext context) {
            _context = context;
        }

        public Task AddTask(Task task) {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public void DeleteTaskById(int id) {
            var task = _context.Tasks.Find(id);
            if (task != null) {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public Task? GetTaskById(int id) {
            return _context.Tasks.Find(id);
        }

        public List<Task> GetTasks() {
            return _context.Tasks.ToList();
        }

        public List<Task> GetTasksByProjectId(int id) {
            return _context.Tasks
                .Where(t => t.ProjectId == id)
                .ToList();
        }
    }
    
}
