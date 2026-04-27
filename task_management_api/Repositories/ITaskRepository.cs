using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllByUserIdAsync(int userId);
    Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId);
    Task AddAsync(TaskItem taskItem);
    void Delete(TaskItem taskItem);
    Task SaveChangesAsync();
}
