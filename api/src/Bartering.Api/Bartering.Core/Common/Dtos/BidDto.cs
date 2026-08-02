using System.Text.Json.Serialization;
using Bartering.Data.Models;

namespace Bartering.Core.Common.Dtos;

[method: JsonConstructor]
public record BidDto(int Id, DateTime? PostedDate, ItemDto? Item, int? ListingId)
{
    public BidDto(Bid bid)
        : this(bid.Id, bid.PostedDate, bid.Item is not null ? new ItemDto(bid.Item) : null, bid.ListingId) { }
}
