using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

// Any class needing to interact with the data base will always be a child of a database context class.
public class  GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)// Acts as the data context
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();
}