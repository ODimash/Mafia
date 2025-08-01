namespace Mafia.Games.API.Tokens.GameToken;

public class GameTokenOptions
{
	public string Issuer { get; set; } = default!;
	public string Audience { get; set; } = default!;
	public string SecretKey { get; set; } = default!;
}
