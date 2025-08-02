using Mafia.Games.API;
using Mafia.User.API;
using Mafia.User.API.Controllers;
using Scalar.AspNetCore;
using Mafia.Games.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddUsersModule();
builder.Services.AddSwaggerGen();
builder.Services.AddGamesModule(builder.Configuration);


var app = builder.Build();
// 2) Подключаем «модули»
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
