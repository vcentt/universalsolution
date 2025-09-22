using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversalSolution.Domain.DTOs;
using UniversalSolution.Domain.Validators;
using UniversalSolution.Services.ExternalApiServices;

namespace UniversalSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExternalController : ControllerBase
{
    private readonly IExternalApiService _externalApiService;
    private readonly ILogger<ExternalController> _logger;
    
    public ExternalController(IExternalApiService externalApiService, ILogger<ExternalController> logger)
    {
        _externalApiService = externalApiService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _externalApiService.GetPostsAsync();
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var validator = new PostValidator();
        var validationResult = await validator.ValidateAsync(createPostDto);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var post = await _externalApiService.CreatePostAsync(createPostDto);
        return Ok(post);
    }
}