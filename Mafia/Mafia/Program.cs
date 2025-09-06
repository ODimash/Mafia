using Mafia.Games.API;
using Mafia.User.API;
using Mafia.User.API.Controllers;
using Scalar.AspNetCore;
using Mafia.Games.API.Controllers;
using Mafia.Games.API.Hubs;
using Mafia.Lobby.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddGamesModule(builder.Configuration);
builder.Services.AddLobbyModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/hub/game");  
app.Run();
