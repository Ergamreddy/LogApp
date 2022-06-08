using main project.Models;
using main project.Repositories;
using Microsoft.AspNetCore.Mvc;
using main project.DTOs;
using Microsoft.AspNetCore.Authorization;
using main project.Utilities;
using System.Security.Claims;

namespace main project.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _Comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository Comment)
    {
        _logger = logger;
        _Comment = Comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment([FromQuery] int id, [FromBody] CommentCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        int PostId = id;
        // var Comments = await _Comment.GetById(userId);
        // if(Comments.Count >= 5)
        // return BadRequest("You can't create another Comment");

        var toCreateItem = new Comment
        {
            Text = Data.Text.Trim(),
            UserId = userId,
            PostId = PostId
        };

        // Insert into DB
        var createdItem = await _Comment.Create(toCreateItem);

        // Return the created Comment
        return StatusCode(201, createdItem);
    }


    [HttpDelete("id")]
    public async Task<ActionResult> DeleteComment([FromQuery] int id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _Comment.GetById(id);
        Console.WriteLine("existing" + existingItem);
        if (existingItem is null)
            return NotFound();

        if (existingItem.Id != userId)
            return StatusCode(403, "You cannot delete other's Comment");

        await _Comment.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetAllComments([FromQuery] int id)
    {
        var allComment = await _Comment.GetAll(id);
        return Ok(allComment);
    }
}