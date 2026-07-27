namespace GameStore.Api.Dtos;

public record class GameDetailsDto // Dto stands for data transfer object
(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);