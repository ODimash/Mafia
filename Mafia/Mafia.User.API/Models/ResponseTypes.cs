using Mafia.User.Application.DTOs;

namespace Mafia.User.API.Models;

public record GetUserResponseData(UserDto User);
public record AuthResponse(string AccessToken);