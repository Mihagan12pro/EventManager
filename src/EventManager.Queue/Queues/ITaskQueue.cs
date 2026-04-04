using EventManager.Queues.ApplicationTasks;

namespace EventManager.Queues.Queues
{
    public interface ITaskQueue<TTask> where TTask : ApplicationTask
    {
        void Enqueue(TTask task);

        bool TryDequeue(out TTask task);
    }
}
