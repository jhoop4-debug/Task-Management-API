using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Tasks;

public class UpdateTaskRequest
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(30)]
    public string Status { get; set; } = "Todo";

    public DateTime? DueDate { get; set; }
}
