namespace UniversalSolution.Domain.DTOs;

public class CreatePostDto
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}