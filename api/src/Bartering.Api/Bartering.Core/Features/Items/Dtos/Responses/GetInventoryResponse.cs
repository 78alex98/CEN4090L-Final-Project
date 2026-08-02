namespace Bartering.Core.Features.Items.Dtos.Responses;

public record GetInventoryResponse(int Id, string Name, string? Description, string? Image, bool IsListed);
