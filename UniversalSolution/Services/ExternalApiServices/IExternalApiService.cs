using UniversalSolution.Domain.DTOs;

namespace UniversalSolution.Services.ExternalApiServices;

public interface IExternalApiService
{
    Task<List<Post>> GetPostsAsync();
    Task<Post> CreatePostAsync(CreatePostDto postDto);
}