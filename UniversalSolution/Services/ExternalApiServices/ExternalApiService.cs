using System.Text.Json;
using UniversalSolution.Domain.DTOs;
using UniversalSolution.Middleware.Exceptions;

namespace UniversalSolution.Services.ExternalApiServices;

public class ExternalApiService : IExternalApiService
{
    private readonly IHttpClientFactory _http;

    public ExternalApiService(IHttpClientFactory http)
    {
        _http = http; 
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        var client = _http.CreateClient("jsonplaceholder");
        var response = await client.GetAsync("posts");

        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalApiException(
                $"Failed to fetch posts. Status: {response.StatusCode}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var posts = JsonSerializer.Deserialize<List<Post>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (posts is null)
        {
            throw new NotFoundException("API returned a null list of posts.");
        }

        return posts;
    }

    public async Task<Post> CreatePostAsync(CreatePostDto postDto)
    {
        var client = _http.CreateClient("jsonplaceholder");
        var response = await client.PostAsJsonAsync("posts", postDto);

        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalApiException(
                $"Failed to create post. Status: {response.StatusCode}");
        }

        var createdPost = await response.Content.ReadFromJsonAsync<Post>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (createdPost is null)
        {
            throw new ExternalApiException("API returned a null post after creation.");
        }

        return createdPost;
    }
}