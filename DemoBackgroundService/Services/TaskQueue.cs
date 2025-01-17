using DemoBackgroundService.Models;
using System.Threading.Channels;

namespace DemoBackgroundService.Services
{
    public class TaskQueue : ITaskQueue
    {
        private readonly Channel<TaskItem> _queue;

        public TaskQueue()
        {
            _queue = Channel.CreateUnbounded<TaskItem>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        public void Enqueue(TaskItem task)
        {
            if (!_queue.Writer.TryWrite(task))
            {
                throw new InvalidOperationException("Task queue is full.");
            }
        }
        public async Task<TaskItem> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
