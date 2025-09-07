using System.Security.Claims;

namespace Mafia.Shared.Infrastructure.Extensions;

public static class ClaimPrincipalExtensions
{
	public static Guid GetUserId(this ClaimsPrincipal? principal)
	{
		var value = principal?.FindFirstValue("userId");
		return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;
	}
}
