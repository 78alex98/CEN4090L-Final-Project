using System.Text.Json.Serialization;
using Bartering.Data.Models;

namespace Bartering.Core.Common.Dtos;

[method: JsonConstructor]
public record ListingDto(
    int Id,
    DateTime? PostedDate,
    DateTime? ClosedDate,
    string? Description,
    string? Message,
    bool? IsOpen,
    ItemDto? Item,
    IEnumerable<BidDto>? Bids,
    BidDto? WinningBid
)
{
    public ListingDto(Listing listing)
        : this(
            listing.Id,
            listing.PostedDate,
            listing.ClosedDate,
            listing.Description,
            listing.Message,
            listing.IsOpen,
            listing.Item is not null ? new ItemDto(listing.Item) : null,
            listing.Bids is not null ? listing.Bids.ToList().ConvertAll(b => new BidDto(b)) : null,
            listing.WinningBid is not null ? new BidDto(listing.WinningBid) : null
        ) { }
}
