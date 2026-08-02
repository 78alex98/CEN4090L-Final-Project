using Bartering.Core.Features.Bids.Dtos.Responses.ChildDtos;

namespace Bartering.Core.Features.Bids.Dtos.Responses;

public record GetBidResponse(int Id, int ListingId, DateTime PostedDate, BidItem Item, string User);
