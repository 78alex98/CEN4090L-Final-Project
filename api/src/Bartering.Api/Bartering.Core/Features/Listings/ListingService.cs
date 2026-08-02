using Bartering.Core.Features.Listings.Dtos;
using Bartering.Core.Features.Listings.Dtos.Requests;
using Bartering.Core.Features.Listings.Dtos.Responses;
using Bartering.Data.Database;
using Bartering.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Core.Features.Listings;

public class ListingService(BarteringDbContext dbContext) : IListingService
{
    public async Task<IEnumerable<ListListingsResponse>> ListListings()
    {
        var listings = await dbContext
            .Listings.AsNoTracking()
            .Include(l => l.Item)
            .ThenInclude(i => i.User)
            .OrderByDescending(l => l.PostedDate)
            .ToListAsync();

        return listings.ConvertAll(l => l.ToListListingsResponse());
    }

    public async Task<GetListingResponse?> GetListing(int listingId)
    {
        var listing = await dbContext
            .Listings.AsNoTracking()
            .Include(l => l.Bids)
            .ThenInclude(b => b.Item)
            .ThenInclude(i => i.User)
            .Include(l => l.WinningBid)
            .ThenInclude(b => b.Item)
            .ThenInclude(i => i.User)
            .Include(l => l.Item)
            .ThenInclude(i => i.User)
            .FirstOrDefaultAsync(l => l.Id == listingId);

        return listing?.ToGetListingResponse();
    }

    public async Task<CreateListingResponse?> CreateListing(CreateListingRequest request, string userId)
    {
        var valid = await ValidateItem(request.ItemId, userId);
        if (!valid)
            return null;

        var item = await dbContext.Items.Include(i => i.User).FirstOrDefaultAsync(i => i.Id == request.ItemId);
        var listing = new Listing
        {
            Title = request.Title ?? item!.Name,
            Description = request.Description ?? string.Empty,
            Message = request.Message ?? string.Empty,
            IsOpen = request.IsOpen,
            ItemId = request.ItemId,
        };

        dbContext.Listings.Add(listing);
        await dbContext.SaveChangesAsync();

        // Since the item was loaded before, the Item property will be eagerly loaded, so no need to explicitly load it here.
        return listing.ToCreateListingResponse();
    }

    public async Task<UpdateListingResponse?> UpdateListing(UpdateListingRequest request, int listingId, string userId)
    {
        var listing = await dbContext
            .Listings.Include(l => l.Item)
            .ThenInclude(i => i.User)
            .FirstOrDefaultAsync(l => l.Id == listingId && l.Item.UserId == userId);

        if (listing is null)
            return null;

        if (!string.IsNullOrEmpty(request.Title))
            listing.Title = request.Title;

        if (!string.IsNullOrEmpty(request.Description))
            listing.Description = request.Description;

        if (!string.IsNullOrEmpty(request.Message))
            listing.Message = request.Message;

        if (request.IsOpen is not null && request.IsOpen != listing.IsOpen)
        {
            if (request.IsOpen is true)
            {
                listing.WinningBidId = null;
                listing.ClosedDate = null;
                listing.IsOpen = true;
            }
            else
            {
                listing.ClosedDate = DateTime.UtcNow;
                listing.IsOpen = false;
            }
        }

        await dbContext.SaveChangesAsync();
        return listing.ToUpdateListingResponse();
    }

    public async Task<bool> DeleteListing(int listingId, string userId)
    {
        var listing = await dbContext.Listings.FirstOrDefaultAsync(l => l.Id == listingId && l.Item.UserId == userId);

        if (listing is null)
            return false;

        dbContext.Listings.Remove(listing);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ValidateItem(int itemId, string userId)
    {
        var userOwnsItem = await dbContext.Items.AnyAsync(i => i.Id == itemId && i.UserId == userId);

        if (!userOwnsItem)
            return false;

        var listingAlreadyExists = await dbContext.Listings.AsNoTracking().AnyAsync(l => l.ItemId == itemId);

        return !listingAlreadyExists;
    }
}
