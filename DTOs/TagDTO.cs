using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace main project.DTOs;



public record PostDTO
{

    [Required]
    [JsonPropertyName("title")]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }

    [Required]
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }


    [Required]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
}


public record PostCreateDTO
{

    [Required]
    [JsonPropertyName("title")]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }




    [Required]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
}



public record PostUpdateDTO
{


    [JsonPropertyName("title")]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }


}