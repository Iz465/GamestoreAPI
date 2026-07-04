namespace GameStore.Api.Dtos;

public record class GameDto // Dto stands for data transfer object
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);