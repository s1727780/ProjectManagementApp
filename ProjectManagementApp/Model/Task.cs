using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model
{
    public class Task{
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int? ProjectId { get; set; }
    }

    public class TaskContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {

        }
    }

    interface ITaskService
    {
        Task? GetTaskById(int id);
        List<Task> GetTasks();
        void DeleteTaskById(int id);
        Task AddTask(Task task);
        List<Task> GetTasksByProjectId(int id);
    }

}
