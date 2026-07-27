
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    // this functions is set up to update the data base or if it doesnt exist, create it.
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope(); // creates a scope (like a key) that gives you access to the database services
        var dbContext = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate(); // creates the data base if it doesn't exist and applies any pending migrations

    }

    public static void AddGameStoreDB(this WebApplicationBuilder builder)
    {

        // this name should be what the data base will be called
        var connString = builder.Configuration.GetConnectionString("GameStore");

        // DB Context has a scoped service lifetime because:
        // 1. It ensures that a new instance of dbcontext is created per request. T
        // 2. We do this because DB connections are a limited and expensive request
        // DBContext is not thread safe. a single request could lead to concurrency requests
        // Easier to manage transactions and ensure data consistency
        // Reusing a dbcontext instance can lead to increased memory usage & worse performance
        builder.Services.AddSqlite<GameStoreContext>
            (connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            { // if statement for if no data found in genre table rows.
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange
                    (
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Platformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" }
                    );
                    context.SaveChanges();
                }
            }));

    }
}

