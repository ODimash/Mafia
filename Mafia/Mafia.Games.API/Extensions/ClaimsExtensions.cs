using System.Security.Claims;

namespace Mafia.Games.API.Extensions;

public static class ClaimsExtensions
{
	public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
	{
		var userIdAsString = claimsPrincipal.FindFirstValue("UserId");
		if (string.IsNullOrEmpty(userIdAsString))
			return Guid.Empty;
		
		var isValidId = Guid.TryParse(userIdAsString, out var userId);
		if (!isValidId)
			return Guid.Empty;
		
		return userId;
	}
}
