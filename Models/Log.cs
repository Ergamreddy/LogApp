using main project.DTOs;

namespace main project.Models;

public record Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }

    public int PostId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }



    public CommentDTO asDto => new CommentDTO
    {
        Text = Text,
        CreatedAt = CreatedAt,
        UpdatedAt = UpdatedAt,
        UserId = UserId,
        PostId = PostId,
    };
}