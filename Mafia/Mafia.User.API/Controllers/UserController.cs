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
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(new { user.Id, user.UserName, user.Email });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        await _service.CreateUserAsync(request.UserName, request.Email);
        return Ok();
    }

    [HttpPut("{id:guid}/email")]
    public async Task<IActionResult> ChangeEmail(Guid id, [FromBody] ChangeEmailRequest request)
    {
        await _service.ChangeEmailAsync(id, request.NewEmail);
        return Ok();
    }
}

public record CreateUserRequest(string UserName, string Email);
public record ChangeEmailRequest(string NewEmail);
