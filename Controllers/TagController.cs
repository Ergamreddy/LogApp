using main project.Models;
using main project.Repositories;
using Microsoft.AspNetCore.Mvc;
using main project.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.VisualBasic;
using main project.Utilities;

namespace main project.Controllers;

[ApiController]
[Authorize]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository _Post;

    public PostController(ILogger<PostController> logger,
    IPostRepository Post)
    {
        _logger = logger;
        _Post = Post;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var toCreateItem = new Post
        {
            Title = Data.Title.Trim(),
            UserId = userId,
        };

        // Insert into DB
        var createdItem = await _Post.Create(toCreateItem);

        // Return the created Post
        return StatusCode(201, createdItem);
    }

    [HttpPut("id")]
    public async Task<ActionResult> UpdatePost(
    [FromBody] PostUpdateDTO Data)
    {
        var Id = GetUserIdFromClaims(User.Claims);

        var existingItem = await _Post.GetById(Id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.Id != Id)
            return StatusCode(403, "You cannot update other's Post");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim()
        };

        await _Post.Update(toUpdateItem);

        return NoContent();
    }

    [HttpDelete("id")]
    public async Task<ActionResult> DeletePost([FromQuery] int id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _Post.GetById(id);
        Console.WriteLine("existing" + existingItem);
        if (existingItem is null)
            return NotFound();

        if (existingItem.Id != userId)
            return StatusCode(403, "You cannot delete other's Post");

        await _Post.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAllPosts()
    {
        var allPost = await _Post.GetAll();
        return Ok(allPost);
    }
}