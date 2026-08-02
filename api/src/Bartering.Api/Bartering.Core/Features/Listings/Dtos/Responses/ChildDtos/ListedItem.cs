namespace Bartering.Core.Features.Listings.Dtos.Responses.ChildDtos;

public record ListedItem(int Id, string Name, string? Description, string? Image, string Owner);
