using Mafia.User.Application.DTOs;
using Mafia.User.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mafia.User.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> Get(Guid id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        await _service.CreateUserAsync(request.UserName, request.Email, request.Password);
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _service.LoginAsync(request.Email, request.Password);
        if (result.IsFailed) return BadRequest(result.Errors);
        return Ok();
    }


}

public record LoginRequest(string Email, string Password);
public record CreateUserRequest(string UserName, string Email, string Password);
