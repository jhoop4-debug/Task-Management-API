using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Tasks
            .Where(task => task.UserId == userId)
            .OrderBy(task => task.Id)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(task => task.Id == id && task.UserId == userId);
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        await _context.Tasks.AddAsync(taskItem);
    }

    public void Delete(TaskItem taskItem)
    {
        _context.Tasks.Remove(taskItem);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
