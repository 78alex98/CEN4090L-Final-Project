namespace Bartering.Core.Features.Listings.Dtos.Responses.ChildDtos;

public record ListedBid(int Id, DateTime PostedDate, ListedBidItem Item, string User);
