using Microsoft.EntityFrameworkCore;
using UniversalSolution.Domain.DTOs;
using UniversalSolution.Domain.Entities;
using UniversalSolution.Infrastructure;
using UniversalSolution.Middleware.Exceptions;
using UniversalSolution.Services.AuthServices;
using UniversalSolution.Services.JwtServices;

namespace UniversalSolution.Services.UserServices;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly IJwtService _jwtService;

    public UserService(ApplicationDbContext db, IJwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    public async Task<UserResponseDto?> CreateUserAsync(RegisterDto newUser)
    {
        if (await EmailExistsAsync(newUser.Email))
        {
            throw new ConflictException("The email address is already registered.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = newUser.Name,
            Email = newUser.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
        };

        var token = _jwtService.CreateToken(user);
        user.Token = token;

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<UserResponseDto> LoginAsync(LoginDto login)
    {
        var user = await GetUserByEmailAsync(login.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials");
        }
        
        var jwt = _jwtService.CreateToken(user);
            
        user.Token = jwt;
        await _db.SaveChangesAsync();
        
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = user.Token
        };
    }
    
    private async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
    
    private async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}