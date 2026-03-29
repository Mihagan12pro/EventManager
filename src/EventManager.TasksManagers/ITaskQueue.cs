namespace EventManager.TasksManagers
{
    public interface ITaskQueue<TTask>
    {
        void Enqueue(TTask task);

        bool TryDequeue(out TTask task);
    }
}
