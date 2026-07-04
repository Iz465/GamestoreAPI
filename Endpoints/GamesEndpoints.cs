using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints; // this is the ID of the class. Each class has their own unique ID 

public static class GamesEndpoints 
{

    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games =
[
    new (
        1,
        "Diablo",
        "ARPG",
        19.99M,
        new DateOnly(1999, 7, 15)),

    new (
        2,
        "Oblivion",
        "RPG",
        40.99M,
        new DateOnly(2006, 4, 7)),

    new (
        3,
        "DOOM 2016",
        "FPS",
        30.99M,
        new DateOnly(2016, 3, 9))
];
    // all extension methods are static
    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        // GET /games - this will get the data for all the games
        group.MapGet("/", () => games);


        // GET specific game id.
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game); // if game is not null returns the game.
                                                                         // if null, returns results not found
        })
            .WithName(GetGameEndpointName);
        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {

            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
                );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games   will update specific games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id); // holds the position of the game in the games list.

            if (index == -1) // -1 means index not found
            {
                return Results.NotFound();
            }

            games[index] = new GameDto
            (
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }
}