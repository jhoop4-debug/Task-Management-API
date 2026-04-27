using TaskManagementApi.DTOs.Tasks;

namespace TaskManagementApi.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskItemResponse>> GetAllAsync(int userId);
    Task<TaskItemResponse?> GetByIdAsync(int id, int userId);
    Task<TaskItemResponse> CreateAsync(CreateTaskRequest request, int userId);
    Task<TaskItemResponse?> UpdateAsync(int id, UpdateTaskRequest request, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}
