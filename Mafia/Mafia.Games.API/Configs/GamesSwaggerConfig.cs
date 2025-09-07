using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Mafia.Games.API.Configs;
public class GameSwaggerConfig : IConfigureOptions<SwaggerGenOptions>
{
	public void Configure(SwaggerGenOptions options)
	{
		options.SwaggerDoc("games", new OpenApiInfo
		{
			Title = "Game API",
			Version = "v1"
		});

		options.AddSecurityDefinition("Game-Token", new OpenApiSecurityScheme
		{
			Name = "Game-Token",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Description = "Введите Game Token"
		});
	}
}