using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mafia.Games.API.Tokens.GameToken;

public interface IGameTokenManager
{
	ClaimsPrincipal Validate(string token);
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
}