using Mafia.Shared.API.Models;
using Mafia.User.API.Models;
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
    public async Task<ResponseModel<GetUserResponseData>> Get(Guid id)
    {
        var result = await _service.GetUserAsync(id);
        return result.ToResponse(x => new GetUserResponseData(x));
    }

    [HttpPost("Registration")]
    public async Task<ResponseModel> Create([FromBody] CreateUserRequest request)
    {
        var result = await _service.CreateUserAsync(request.UserName, request.Email, request.Password);
        return result.ToEmptyResponse();
    }

    [HttpPost("Login")]
    public async Task<ResponseModel<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _service.LoginAsync(request.Email, request.Password);
        return result.ToResponse(x => new AuthResponse(result.Value));
    }


}


