using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(new { message = "User ID was missing from the token somehow." });
        }

        var tasks = await _taskService.GetAllAsync(userId.Value);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(new { message = "User ID was missing from the token somehow." });
        }

        var task = await _taskService.GetByIdAsync(id, userId.Value);
        if (task is null)
        {
            return NotFound(new { message = "Task not found." });
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(new { message = "User ID was missing from the token somehow." });
        }

        var createdTask = await _taskService.CreateAsync(request, userId.Value);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(new { message = "User ID was missing from the token somehow." });
        }

        var updatedTask = await _taskService.UpdateAsync(id, request, userId.Value);
        if (updatedTask is null)
        {
            return NotFound(new { message = "Task not found." });
        }

        return Ok(updatedTask);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(new { message = "User ID was missing from the token somehow." });
        }

        var deleted = await _taskService.DeleteAsync(id, userId.Value);
        if (!deleted)
        {
            return NotFound(new { message = "Task not found." });
        }

        return Ok(new { message = "Task deleted successfully." });
    }

    private int? GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
