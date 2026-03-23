using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<bool> RegisterAsync(RegisterUserDto dto);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
