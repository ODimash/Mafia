using Mafia.Games.API.Tokens.GameToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Mafia.Games.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class GameAuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var request = context.HttpContext.Request;
		if (!request.Headers.TryGetValue("Game-Token", out var token))
		{
			context.Result = new UnauthorizedResult();
			return;
		}

		var tokenManager = context.HttpContext.RequestServices
			.GetRequiredService<IGameTokenManager>();

		try
		{
			ClaimsPrincipal principal = tokenManager.Validate(token.ToString());
			context.HttpContext.User = principal;
		}
		catch
		{
			context.Result = new UnauthorizedResult();
		}
	}
}