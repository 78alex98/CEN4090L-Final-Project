using System.ComponentModel;

namespace Bartering.Core.Features.Listings.Dtos.Requests;

public record UpdateListingRequest(
    [property: Description("The title of the listing.")] string? Title = null,
    [property: Description("A description of the listing.")] string? Description = null,
    [property: Description("The message to be sent to the winner.")] string? Message = null,
    [property: Description("Whether or not the listing is open for bidding.")] bool? IsOpen = null
);
