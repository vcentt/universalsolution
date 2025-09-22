using Microsoft.AspNetCore.Mvc;
using UniversalSolution.Domain.DTOs;
using UniversalSolution.Services.AuthServices;

namespace UniversalSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _auth;

    public UserController(IUserService auth)
    {
        _auth = auth;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _auth.CreateUserAsync(dto);

        return Ok(user);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _auth.LoginAsync(loginDto);
        return Ok(result);
    }
}