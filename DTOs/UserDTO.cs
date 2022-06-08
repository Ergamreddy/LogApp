using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace main project.DTOs;



public record UserDTO
{

    [Required]
    [JsonPropertyName("full_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string FullName { get; set; }
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}


public record UserRegisterDTO
{

    [Required]
    [JsonPropertyName("full_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string FullName { get; set; }
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}


public record UserUpdateDTO
{


    [JsonPropertyName("full_name")]
    public string FullName { get; set; }


}