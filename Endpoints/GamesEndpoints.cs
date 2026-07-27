using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints; // this is the ID of the class. Each class has their own unique ID 

public static class GamesEndpoints 
{

    const string GetGameEndpointName = "GetGame";

  
    // all extension methods are static
    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        // GET /games - this will get the data for all the games
        group.MapGet("/", async (GameStoreContext dbContext) => 
        await dbContext.Games.
        Include(game => game.Genre).
        Select(game => new GameSummaryDto
        (
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        ))
        .AsNoTracking() // doesnt keep track of the entities loaded in memory
        .ToListAsync()); // tells the database to return the game dto info in a list.


        // GET specific game id.
        group.MapGet("/{id}",  async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id); // find the game in the database with the id that was passed in.

            return game is null ? Results.NotFound() : Results.Ok( // if game is not null returns the gameDTO.
                new GameDetailsDto
                (
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
                ); 
                                                                         // if null, returns results not found
        })
            .WithName(GetGameEndpointName);
        // POST /games // allows a new instance to happen each POST request
       // async tells it the request will be using async logic
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            // stores the newgame DTO data into the game class
            //Note that it does not have an id as the database will generate that
            Game game = new()
            { 
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate

            };

            dbContext.Games.Add(game); // tells the database that a new game needs to be added to the database

            await dbContext.SaveChangesAsync();  // actually adds the game to the database and saves it. 
      
            // this now has an id as the database has added & saved the game and created an id for it.
            GameDetailsDto gameDto = new
            (
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto); // have it return a DTO. 
        }); // destroys gamestorecontext an end of request

        // PUT /games   will update specific games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame,
            GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null) 
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).
            ExecuteDeleteAsync(); // deletes the game with the id that was passed in.
            // no save changes needed as the database is already updated with the delete.
            return Results.NoContent();
        });
    }
}