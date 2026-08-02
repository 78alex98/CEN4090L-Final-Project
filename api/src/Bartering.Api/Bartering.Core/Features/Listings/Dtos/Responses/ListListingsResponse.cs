using Bartering.Core.Features.Listings.Dtos.Responses.ChildDtos;

namespace Bartering.Core.Features.Listings.Dtos.Responses;

public record ListListingsResponse(
    int Id,
    string Title,
    string? Description,
    string? Message,
    bool IsOpen,
    DateTime PostedDate,
    DateTime? ClosedDate,
    ListedItem Item
);
