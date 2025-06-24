using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementApp;
using ProjectManagementApp.Data;
using ProjectManagementApp.Model;
using ProjectManagementApp.Service;
using Task = ProjectManagementApp.Model.Task;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

// - Register service

builder.Services.AddScoped<ITaskService, EfCoreTaskService>();


builder.Services.AddDbContext<TaskContext>(options =>
{
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true");
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:5001/")
});

WebApplication app = builder.Build();

DbInitialiser.CreateDbIfNotExists(app);
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
    return TypedResults.Created("/tasks/{task.Id}", task);

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

app.MapPut("/tasks/{id}", (Task task, ITaskService service) => {
    Task t = service.UpdateTask(task);
    return TypedResults.Ok(t);
});

app.MapDelete("/tasks/{id}", (int id, ITaskService service) =>
{
    service.DeleteTaskById(id);
    return TypedResults.NoContent();
});


#endregion


app.Run();

