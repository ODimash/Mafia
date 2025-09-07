using Mafia.Chats.API.ModuleExtensions;
using Mafia.Games.API;
using Mafia.Games.API.Hubs;
using Mafia.Lobby.API;
using Mafia.User.API;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGamesModule(builder.Configuration);
builder.Services.AddLobbyModule();
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddChatModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/users/swagger.json", "Users API v1");
		c.SwaggerEndpoint("/swagger/games/swagger.json", "Games API v1");
	});
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/hub/game");
app.Run();
