using Bartering.Core.Features.Bids.Dtos;
using Bartering.Core.Features.Bids.Dtos.Requests;
using Bartering.Core.Features.Bids.Dtos.Responses;
using Bartering.Data.Database;
using Bartering.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Core.Features.Bids;

public class BidService(BarteringDbContext dbContext) : IBidService
{
    public async Task<GetBidResponse> GetBid(int bidId)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateBidResponse?> CreateBid(CreateBidRequest request, int listingId, string userId)
    {
        var valid = await ValidateItem(request.ItemId, userId);
        if (!valid)
            return null;

        var bid = new Bid { ItemId = request.ItemId, ListingId = listingId };

        dbContext.Bids.Add(bid);
        await dbContext.SaveChangesAsync();

        await dbContext.Entry(bid).Reference(b => b.Item).Query().Include(i => i.User).LoadAsync();

        return bid.ToCreateBidResponse();
    }

    public async Task<bool> DeleteBid(int listingId, int bidId, string userId)
    {
        var bid = await dbContext.Bids.FirstOrDefaultAsync(b =>
            b.Id == bidId && b.ListingId == listingId && b.Item.UserId == userId
        );

        if (bid is null)
            return false;

        dbContext.Bids.Remove(bid);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SelectWinningBid(int listingId, int bidId, string listingOwnerId)
    {
        // Item data is loaded here to support the item swapping and listing/bid deletion behaviour. Refer to Issue #274.
        var listing = await dbContext
            .Listings.Include(l => l.Item).
            Include(l => l.Bids.Where(b => b.Id == bidId)).ThenInclude(b => b.Item)
            .FirstOrDefaultAsync(l => l.Id == listingId && l.Item.UserId == listingOwnerId);

        if (listing is null || !listing.IsOpen || listing.Bids.Count != 1)
            return false;

        listing.WinningBidId = bidId;
        listing.ClosedDate = DateTime.UtcNow;
        listing.IsOpen = false;

        // The behaviour below is not of the original intention, but is being done due to internal discussion regarding time constraints.
        // Refer to Issue #274 in the repository.
        var bid = listing.Bids.Single();
        var listedItemUserId = listing.Item.UserId;
        var bidItemUserId = bid.Item.UserId;
        listing.Item.UserId = bidItemUserId;
        bid.Item.UserId = listedItemUserId;
        
        dbContext.Listings.Remove(listing);
        
        await dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<bool> ValidateItem(int itemId, string userId)
    {
        var userOwnsItem = await dbContext.Items.AnyAsync(i => i.Id == itemId && i.UserId == userId);

        if (!userOwnsItem)
            return false;

        var bidAlreadyExists = await dbContext.Bids.AsNoTracking().AnyAsync(b => b.ItemId == itemId);

        return !bidAlreadyExists;
    }
}
