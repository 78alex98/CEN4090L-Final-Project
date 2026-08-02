using Bartering.Core.Common.Util;
using Bartering.Core.Features.Bids.Dtos.Responses;
using Bartering.Core.Features.Bids.Dtos.Responses.ChildDtos;
using Bartering.Data.Models;

namespace Bartering.Core.Features.Bids.Dtos;

public static class BidDtoMapper
{
    public static CreateBidResponse ToCreateBidResponse(this Bid bid)
    {
        return new CreateBidResponse(
            Id: bid.Id,
            ListingId: bid.ListingId,
            PostedDate: bid.PostedDate,
            Item: bid.Item.ToBidItem(),
            User: bid.Item.User.UserName ?? string.Empty
        );
    }

    public static GetBidResponse ToGetBidResponse(this Bid bid)
    {
        return new GetBidResponse(
            Id: bid.Id,
            ListingId: bid.ListingId,
            PostedDate: bid.PostedDate,
            Item: bid.Item.ToBidItem(),
            User: bid.Item.User.UserName ?? string.Empty
        );
    }

    public static BidItem ToBidItem(this Item item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new BidItem(Id: item.Id, Name: item.Name, Description: item.Description, Image: image);
    }
}
