namespace Mafia.User.API.Models;

public record LoginRequest(string Email, string Password);
public record CreateUserRequest(string UserName, string Email, string Password);