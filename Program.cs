
using Gamestore.Api.Endpoints;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDB();

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.Run(); // app is the host 
