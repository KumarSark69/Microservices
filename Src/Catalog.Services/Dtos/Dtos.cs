using System.ComponentModel.DataAnnotations;

namespace Catalog.Service.DTO
{
    public record ItemDto(Guid Id, string Name, string Description, double Price, DateTimeOffset CreatesDate);
    public record CreateDto([Required]string Name, string Description,[Range(0,1000)] double Price);
    public record UpdateDto([Required]string Name, string Description,[Range(0,1000)] double Price);
}