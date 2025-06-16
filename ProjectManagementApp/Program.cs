using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

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

app.MapGet("/tasks", () => tasks);

app.MapGet("/tasks/{id}", Results<Ok<Task>, NotFound> (int id) =>
{
    Task? targetTask = tasks.SingleOrDefault(t => id == t.id);

    if (targetTask == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(targetTask);

});


app.MapPost("/tasks", (Task task) =>
{
    tasks.Add(task);
    return TypedResults.Created("/tasks/{id}", task);
});

app.MapDelete("/tasks/{id}", (int id) =>
{
    tasks.RemoveAll(t => id == t.id);
    return TypedResults.NoContent();
});


#endregion


app.Run();

public record Task(int id, string Name, DateTime DueDate, bool isCompleted);
