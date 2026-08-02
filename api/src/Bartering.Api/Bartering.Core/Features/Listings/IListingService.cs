using Bartering.Core.Features.Listings.Dtos.Requests;
using Bartering.Core.Features.Listings.Dtos.Responses;

namespace Bartering.Core.Features.Listings;

public interface IListingService
{
    Task<IEnumerable<ListListingsResponse>> ListListings();

    Task<GetListingResponse?> GetListing(int listingId);

    Task<CreateListingResponse?> CreateListing(CreateListingRequest request, string userId);

    Task<UpdateListingResponse?> UpdateListing(UpdateListingRequest request, int listingId, string userId);

    Task<bool> DeleteListing(int listingId, string userId);
}
