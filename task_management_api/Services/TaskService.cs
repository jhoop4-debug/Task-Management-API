using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Todo",
        "In Progress",
        "Done"
    };

    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskItemResponse>> GetAllAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return tasks.Select(MapTask).ToList();
    }

    public async Task<TaskItemResponse?> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);
        return task is null ? null : MapTask(task);
    }

    public async Task<TaskItemResponse> CreateAsync(CreateTaskRequest request, int userId)
    {
        var taskItem = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            Status = NormalizeStatus(request.Status),
            DueDate = request.DueDate,
            UserId = userId
        };

        await _taskRepository.AddAsync(taskItem);
        await _taskRepository.SaveChangesAsync();

        return MapTask(taskItem);
    }

    public async Task<TaskItemResponse?> UpdateAsync(int id, UpdateTaskRequest request, int userId)
    {
        var taskItem = await _taskRepository.GetByIdAndUserIdAsync(id, userId);
        if (taskItem is null)
        {
            return null;
        }

        taskItem.Title = request.Title.Trim();
        taskItem.Description = request.Description?.Trim();
        taskItem.Status = NormalizeStatus(request.Status);
        taskItem.DueDate = request.DueDate;

        await _taskRepository.SaveChangesAsync();
        return MapTask(taskItem);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var taskItem = await _taskRepository.GetByIdAndUserIdAsync(id, userId);
        if (taskItem is null)
        {
            return false;
        }

        _taskRepository.Delete(taskItem);
        await _taskRepository.SaveChangesAsync();
        return true;
    }

    private static string NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return "Todo";
        }

        var cleanedStatus = status.Trim();
        var matchedStatus = ValidStatuses.FirstOrDefault(validStatus =>
            string.Equals(validStatus, cleanedStatus, StringComparison.OrdinalIgnoreCase));

        // If somebody sends a random status, we just fall back instead of causing drama.
        return matchedStatus ?? "Todo";
    }

    private static TaskItemResponse MapTask(TaskItem taskItem)
    {
        return new TaskItemResponse
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            Status = taskItem.Status,
            DueDate = taskItem.DueDate,
            CreatedAt = taskItem.CreatedAt
        };
    }
}
