namespace Mafia.User.Application.Contracts;

public interface IJwtTokenService
{
	public string GenerateToken(string userId, string role);
}
