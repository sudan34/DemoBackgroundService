using Serilog;
using DemoBackgroundService.Models;
using DemoBackgroundService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console() // Log to the console
//    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
//    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog((hostconfig, config) =>
{
    config.ReadFrom.Configuration(hostconfig.Configuration);
    
    // Log only Information and Error levels
    config.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning); // Avoid excessive framework logs
    config.MinimumLevel.Information(); // Global minimum level is Information

    config.WriteTo.Console();
    config.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});

//Add services to the container
builder.Services.AddSingleton<ITaskQueue, TaskQueue>();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Simulate adding tasks via an HTTP endpoint
app.MapPost("/add-task", (ITaskQueue taskQueue) =>
{
    var task = new TaskItem
    {
        Id = Guid.NewGuid(),
        TaskName = $"Task-{Guid.NewGuid()}",
        CreatedAt = DateTime.UtcNow
    };

    taskQueue.Enqueue(task);
    return Results.Ok(new { Message = "Task added successfully", TaskId = task.Id });
});

app.MapGet("/", () => "Advanced Background Service is running!");

app.Run();
