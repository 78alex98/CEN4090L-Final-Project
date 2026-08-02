using Bartering.Core.Common.Dtos;

namespace Bartering.Core.Features.Items.Dtos.Responses;

public record CreateItemResponse(int Id, string Name, string? Description, string? Image, ApplicationUserDto? Owner);
