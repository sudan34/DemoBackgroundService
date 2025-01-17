using DemoBackgroundService.Models;

namespace DemoBackgroundService.Services
{
    public interface ITaskQueue
    {
        void Enqueue(TaskItem task);
        Task<TaskItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
