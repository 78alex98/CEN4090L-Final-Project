using System.Text.Json.Serialization;
using Bartering.Core.Common.Util;
using Bartering.Data.Models;

namespace Bartering.Core.Common.Dtos;

[method: JsonConstructor]
public record ItemDto(int Id, string? Name, string? Description, string? Image, ApplicationUserDto? Owner)
{
    public ItemDto(Item item)
        : this(
            item.Id,
            item.Name,
            item.Description,
            item.Image is not null ? ImageUtility.ConstructDataUrl(item.Image, item.ImageType ?? string.Empty) : null,
            item.User is not null ? new ApplicationUserDto(item.User) : null
        ) { }
}
