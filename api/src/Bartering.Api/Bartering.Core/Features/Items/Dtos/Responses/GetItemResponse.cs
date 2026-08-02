using Bartering.Core.Common.Dtos;

namespace Bartering.Core.Features.Items.Dtos.Responses;

public record GetItemResponse(
    int Id,
    string Name,
    string? Description,
    string? Image,
    bool IsListed,
    ApplicationUserDto? Owner
);
