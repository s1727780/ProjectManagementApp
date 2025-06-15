using Microsoft.AspNetCore.Http.HttpResults;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

WebApplication app = builder.Build();

List<Task> tasks = new List<Task>();

app.MapGet("/tasks", () => tasks);

app.MapGet("/tasks/{id}", Results<Ok<Task>, NotFound<int>> (int id) =>
{
    Task? targetTask = tasks.SingleOrDefault(t => id == t.id);

    if (targetTask == null)
    {
        return TypedResults.NotFound(id);
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

app.Run();

public record Task(int id, string Name, DateTime DueDate, bool isCompleted);
