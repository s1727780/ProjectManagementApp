using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

// - Register service
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskSerivce());

WebApplication app = builder.Build();


// Middleware

// - Built-in
app.UseRewriter(new RewriteOptions().AddRedirect("todos/(.*)", "tasks/$1"));

// - Custom
app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path} {DateTime.UtcNow} Started");
    await next(context);
    Console.WriteLine($"{context.Request.Method} {context.Request.Path} {DateTime.UtcNow} Finished");
});



// API

List<Task> tasks = new List<Task>();

#region Tasks

app.MapGet("/tasks", (ITaskService service) => service.GetTasks());

app.MapGet("/tasks/{id}", Results<Ok<Task>, NotFound> (int id, ITaskService service) =>
{
    Task? targetTask = service.GetTaskById(id);

    if (targetTask == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(targetTask);

});

app.MapPost("/tasks", (Task task, ITaskService service) =>
{
    service.AddTask(task);
    return TypedResults.Created("/tasks/{id}", task);

}).AddEndpointFilter(async (context, next) => {     // Endpoint filter
    Task taskArguement = context.GetArgument<Task>(0);
    Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
    if (taskArguement.DueDate < DateTime.UtcNow)
    {
        errors.Add(nameof(Task.DueDate), ["Cannot have due date in the past"]);
    }

    if (taskArguement.IsCompleted)
    {
        errors.Add(nameof(Task.IsCompleted), ["Cannot add completed task"]);
    }

    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    return await next(context);


});

app.MapDelete("/tasks/{id}", (int id, ITaskService service) =>
{
    service.DeleteTaskById(id);
    return TypedResults.NoContent();
});


#endregion


app.Run();

public record Task(int id, string Name, DateTime DueDate, bool IsCompleted);

interface ITaskService {
    Task? GetTaskById(int id);
    List<Task> GetTasks();
    void DeleteTaskById(int id);
    Task AddTask(Task task);
}


class InMemoryTaskSerivce : ITaskService {
    private readonly List<Task> _tasks = [];
    
    public Task AddTask(Task task) { 
        _tasks.Add(task); 
        return task; 
    }
    
    public void DeleteTaskById(int id) {
        _tasks.RemoveAll(task => id == task.id); 
    }

    public Task GetTaskById(int id) {
        return _tasks.SingleOrDefault(t => id == t.id);
    }

    public List<Task> GetTasks() { 
        return _tasks; 
    }
}