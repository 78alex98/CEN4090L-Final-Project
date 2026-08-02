using Bartering.Core.Features.Bids.Dtos.Requests;
using Bartering.Core.Features.Bids.Dtos.Responses;

namespace Bartering.Core.Features.Bids;

public interface IBidService
{
    Task<GetBidResponse> GetBid(int bidId);

    Task<CreateBidResponse?> CreateBid(CreateBidRequest request, int listingId, string userId);

    Task<bool> DeleteBid(int listingId, int bidId, string userId);

    Task<bool> SelectWinningBid(int listingId, int bidId, string listingOwnerId);
}
