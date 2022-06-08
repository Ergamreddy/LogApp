using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace main project.DTOs;



public record CommentDTO
{

    [Required]
    [JsonPropertyName("text")]
    [MinLength(3)]
    [MaxLength(90)]
    public string Text { get; set; }

    [Required]
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }


    [Required]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [Required]
    [JsonPropertyName("post_id")]
    public int PostId { get; set; }
}


public record CommentCreateDTO
{

    [Required]
    [JsonPropertyName("text")]
    [MinLength(3)]
    [MaxLength(90)]
    public string Text { get; set; }

    // [Required]
    // [JsonPropertyName("user_id")]
    // public int UserId { get; set; }

    // [Required]
    // [JsonPropertyName("post_id")]
    // public int PostId { get; set; }







}