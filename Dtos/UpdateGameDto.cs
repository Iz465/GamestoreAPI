using System.ComponentModel.DataAnnotations;
namespace GameStore.Api.Dtos;



public record UpdateGameDto // Dto stands for data transfer object
(
    [Required][StringLength(50)] string Name,
     [Range(1, 50)] int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);