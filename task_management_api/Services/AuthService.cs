using Microsoft.AspNetCore.Identity;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<(bool Success, string Message, AuthResponse? Data)> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);
        if (existingUser is not null)
        {
            return (false, "That email is already being used.", null);
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalizedEmail,
        };

        // Hashing the password is way better than saving the raw thing. Raw passwords are cursed.
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "User registered successfully.", BuildAuthResponse(user));
    }

    public async Task<(bool Success, string Message, AuthResponse? Data)> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLower());
        if (user is null)
        {
            return (false, "Invalid email or password.", null);
        }

        var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordCheck == PasswordVerificationResult.Failed)
        {
            return (false, "Invalid email or password.", null);
        }

        return (true, "Login successful.", BuildAuthResponse(user));
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        return new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            Name = user.Name,
            Email = user.Email
        };
    }
}
