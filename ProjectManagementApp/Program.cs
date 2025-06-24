using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Model;
using ProjectManagementApp.Service;
using Task = ProjectManagementApp.Model.Task;
using Microsoft.Extensions.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

// - Register service


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
