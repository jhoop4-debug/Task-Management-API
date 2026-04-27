using TaskManagementApi.DTOs.Auth;

namespace TaskManagementApi.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Message, AuthResponse? Data)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message, AuthResponse? Data)> LoginAsync(LoginRequest request);
}
