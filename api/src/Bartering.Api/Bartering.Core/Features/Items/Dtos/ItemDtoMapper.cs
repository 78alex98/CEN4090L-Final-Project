using Bartering.Core.Common.Dtos;
using Bartering.Core.Common.Util;
using Bartering.Core.Features.Items.Dtos.Responses;
using Bartering.Data.Models;

namespace Bartering.Core.Features.Items.Dtos;

public static class ItemDtoMapper
{
    public static CreateItemResponse ToCreateItemResponse(this Item item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new CreateItemResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            Owner: item.User is not null ? new ApplicationUserDto(item.User) : null
        );
    }

    public static GetItemResponse ToGetItemResponse(this Item item, bool isListed = false)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new GetItemResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            IsListed: isListed,
            Owner: item.User is not null ? new ApplicationUserDto(item.User) : null
        );
    }

    public static GetItemResponse ToGetItemResponse(this ItemIsListed item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new GetItemResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            IsListed: item.IsListed,
            Owner: item.User is not null ? new ApplicationUserDto(item.User) : null
        );
    }

    public static GetInventoryResponse ToGetInventoryResponse(this Item item, bool isListed = false)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new GetInventoryResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            IsListed: isListed
        );
    }

    public static GetInventoryResponse ToGetInventoryResponse(this ItemIsListed item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new GetInventoryResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            IsListed: item.IsListed
        );
    }

    public static UpdateItemResponse ToUpdateItemResponse(this Item item)
    {
        var image = item.Image is not null
            ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty)
            : null;

        return new UpdateItemResponse(
            Id: item.Id,
            Name: item.Name,
            Description: item.Description,
            Image: image,
            Owner: item.User is not null ? new ApplicationUserDto(item.User) : null
        );
    }
}
