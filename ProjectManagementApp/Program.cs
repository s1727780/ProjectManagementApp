var builder = WebApplication.CreateBuilder(args);

// Host configuration goes here

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
