using Bartering.Core.Features.Listings.Dtos.Responses.ChildDtos;

namespace Bartering.Core.Features.Listings.Dtos.Responses;

public record UpdateListingResponse(
    int Id,
    string Title,
    string? Description,
    string? Message,
    bool IsOpen,
    ListedItem Item
);
