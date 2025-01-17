namespace DemoBackgroundService.Services
{
    public class Worker : BackgroundService
    {
        private readonly ITaskQueue _taskQueue;
        private readonly ILogger<Worker> _logger;

        public Worker(ITaskQueue taskQueue, ILogger<Worker> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AdvancedBackgroundService started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Wait for a task from the queue
                    var task = await _taskQueue.DequeueAsync(stoppingToken);

                    // Process the task
                    _logger.LogInformation("Processing Task: {taskId}, Name: {taskName}, CreatedAt: {createdAt}",
                        task.Id, task.TaskName, task.CreatedAt);

                    // Simulate processing delay
                    await Task.Delay(2000, stoppingToken);

                    _logger.LogInformation("Task {taskId} processed successfully!", task.Id);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown
                    _logger.LogInformation("AdvancedBackgroundService is stopping...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing a task.");
                }
            }
        }
    }
}
