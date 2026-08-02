using System.ComponentModel;

namespace Bartering.Core.Features.Items.Dtos.Requests;

public record CreateItemRequest(
    [property: Description("The name of the item.")] string Name,
    [property: Description("A description of the item.")] string? Description = null,
    [property: Description("A data URL with a base64 encoded image.")] string? Image = null
);
