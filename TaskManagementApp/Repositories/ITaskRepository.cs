using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateTaskAsync(TaskModel task);
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<TaskModel> UpdateTaskAsync(TaskModel task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
