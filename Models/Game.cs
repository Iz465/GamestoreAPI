namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public Genre? Genre { get; set; }
    // its not nullable therefore a genre id is required
    public int GenreId { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

}