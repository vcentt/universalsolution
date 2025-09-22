using UniversalSolution.Domain.DTOs;
using UniversalSolution.Domain.Entities;

namespace UniversalSolution.Services.AuthServices;

public interface IUserService
{
    Task<UserResponseDto?> CreateUserAsync(RegisterDto newUser);
    Task<UserResponseDto> LoginAsync(LoginDto login);
}