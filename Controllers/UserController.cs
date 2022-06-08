using main project.Models;
using main project.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using main project.DTOs;
using Microsoft.VisualBasic;
using main project.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace main project.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;
    }


    [HttpPost("register")]

    public async Task<ActionResult<UserDTO>> Createuser([FromBody] UserRegisterDTO Data)
    {
        var toCreateuser = new User
        {
            Name = Data.Name.Trim(),
            Email = Data.Email.Trim(),
            HashtagPassword = BCrypt.Net.BCrypt.HashtagPassword(Data.Password),
            Email = Data.Email.Trim(),
            createdAt = DateTime.Now,
            status = "active",
            isSuperUser = false,
        };
        var createduser = await _user.Create(toCreateuser);
        return StatusCode(StatusCodes.Status201Created, createduser.asDto);
        // return Createuser();


    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }



    [HttpPost("login")]
    public async Task<ActionResult<UserLoginDTO>> Login(
        [FromBody] UserLoginDTO Data
    )
    {
        var existingUser = await _user.GetByEmail(Data.Email);

        if (existingUser is null)
            return NotFound();

        if (!BCrypt.Net.BCrypt.Verify(Data.Password, existingUser.Password))
            return BadRequest("Username or password is incorrect");

        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            EmailId = existingUser.EmailId,
            Password = existingUser.Password,
            Token = token,
        };

        return Ok(res);
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(UserConstants.Id, user.Id.ToString()),
            new Claim(UserConstants.Email, user.Email),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [HttpPut("id")]
    [Authorize]
    public async Task<ActionResult> UserUpdateDTO(
    [FromBody] UserUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _user.GetUserById(userId);

        if (existingItem is null)
            return NotFound();

        if (existingItem.Id != userId)
            return StatusCode(403, "You cannot update other's Name");

        var toUpdateItem = existingItem with
        {
            Name = Data.Name is null ? existingItem.Name : Data.Name.Trim(),
        };

        await _user.Update(toUpdateItem);

        return NoContent();
    }
}