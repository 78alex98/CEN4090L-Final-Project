using Bartering.Core.Common.Util;
using Bartering.Core.Features.Listings.Dtos.Responses;
using Bartering.Core.Features.Listings.Dtos.Responses.ChildDtos;
using Bartering.Data.Models;

namespace Bartering.Core.Features.Listings.Dtos;

public static class ListingDtoMapper
{
    public static CreateListingResponse ToCreateListingResponse(this Listing listing)
    {
        return new CreateListingResponse(
            Id: listing.Id,
            Title: listing.Title,
            Description: listing.Description,
            Message: listing.Message,
            IsOpen: listing.IsOpen,
            PostedDate: listing.PostedDate,
            Item: listing.Item.ToListedItem()
        );
    }

    public static GetListingResponse ToGetListingResponse(this Listing listing)
    {
        return new GetListingResponse(
            Id: listing.Id,
            Title: listing.Title,
            Description: listing.Description,
            Message: listing.Message,
            IsOpen: listing.IsOpen,
            PostedDate: listing.PostedDate,
            ClosedDate: listing.ClosedDate,
            Item: listing.Item.ToListedItem(),
            Bids: listing.Bids.ToList().ConvertAll(b => b.ToListedBid()),
            WinningBid: listing.WinningBid is not null ? listing.WinningBid.ToListedBid() : null
        );
    }

    public static ListListingsResponse ToListListingsResponse(this Listing listing)
    {
        return new ListListingsResponse(
            Id: listing.Id,
            Title: listing.Title,
            Description: listing.Description,
            Message: listing.Message,
            IsOpen: listing.IsOpen,
            PostedDate: listing.PostedDate,
            ClosedDate: listing.ClosedDate,
            Item: listing.Item.ToListedItem()
        );
    }

    public static UpdateListingResponse ToUpdateListingResponse(this Listing listing)
    {
        return new UpdateListingResponse(
            Id: listing.Id,
            Title: listing.Title,
            Description: listing.Description,
            Message: listing.Message,
            IsOpen: listing.IsOpen,
            Item: listing.Item.ToListedItem()
        );
    }

    public static ListedItem ToListedItem(this Item item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new ListedItem(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            Owner: item.User.UserName ?? string.Empty
        );
    }

    public static ListedBid ToListedBid(this Bid bid)
    {
        return new ListedBid(
            Id: bid.Id,
            PostedDate: bid.PostedDate,
            Item: bid.Item.ToListedBidItem(),
            User: bid.Item.User.UserName ?? string.Empty
        );
    }

    public static ListedBidItem ToListedBidItem(this Item item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new ListedBidItem(Id: item.Id, Name: item.Name, Description: item.Description, Image: image);
    }
}
