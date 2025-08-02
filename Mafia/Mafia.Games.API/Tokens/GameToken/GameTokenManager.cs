using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mafia.Games.API.Tokens.GameToken;

public interface IGameTokenManager
{
	ClaimsPrincipal Validate(string token);
	string GenerateToken(Guid gameId, Guid playerId);
}

public class GameTokenManager : IGameTokenManager
{
	private readonly GameTokenOptions _options;

	public GameTokenManager(IOptions<GameTokenOptions> options)
	{
		_options = options.Value;
	}

	public ClaimsPrincipal Validate(string gameToken)
	{
		var handler = new JwtSecurityTokenHandler();

		var validation = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = _options.Issuer,
			ValidateAudience = true,
			ValidAudience = _options.Audience,
			ValidateLifetime = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
		};

		try
		{
			return handler.ValidateToken(gameToken, validation, out var _);
		}
		catch (Exception)
		{
			return new ClaimsPrincipal();
		}
	}
	public string GenerateToken(Guid gameId, Guid playerId)
	{
		var claims = new[]
		{
			new Claim("GameId", gameId.ToString()), 
			new Claim("PlayerId", playerId.ToString()),
		};
		
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var tokenDescriptor = new JwtSecurityToken(
			issuer: _options.Issuer, 
			audience: _options.Audience, 
			claims: claims, 
			signingCredentials: creds);
		
		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}
}
