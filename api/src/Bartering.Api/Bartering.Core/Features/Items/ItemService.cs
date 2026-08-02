using Bartering.Core.Features.Items.Dtos;
using Bartering.Core.Features.Items.Dtos.Requests;
using Bartering.Core.Features.Items.Dtos.Responses;
using Bartering.Data.Database;
using Bartering.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Core.Features.Items;

public class ItemService(BarteringDbContext dbContext) : IItemService
{
    public async Task<GetItemResponse?> GetItem(int itemId, string userId)
    {
        var item = await dbContext
            .ItemIsListed.AsNoTracking()
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == itemId && i.UserId == userId);

        return item?.ToGetItemResponse();
    }

    public async Task<IEnumerable<GetInventoryResponse>> GetInventory(string userId)
    {
        var items = await dbContext
            .ItemIsListed.AsNoTracking()
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.Id)
            .ToListAsync();

        return items.ConvertAll(i => i.ToGetInventoryResponse());
    }

    public async Task<CreateItemResponse> CreateItem(CreateItemRequest request, string userId)
    {
        var imageData = ParseDataUrl(request.Image);

        var item = new Item
        {
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            Image = imageData.Base64,
            ImageType = imageData.MediaType,
            UserId = userId,
        };

        dbContext.Items.Add(item);
        await dbContext.SaveChangesAsync();

        return item.ToCreateItemResponse();
    }

    public async Task<UpdateItemResponse?> UpdateItem(UpdateItemRequest request, string userId)
    {
        var item = await dbContext
            .Items.Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == request.Id && i.UserId == userId);

        if (item is null)
            return null;

        if (!string.IsNullOrEmpty(request.Name))
            item.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Description))
            item.Description = request.Description;

        var (mediaType, base64Data) = ParseDataUrl(request.Image);
        if (mediaType is not null && base64Data is not null)
        {
            item.Image = base64Data;
            item.ImageType = mediaType;
        }

        await dbContext.SaveChangesAsync();
        return item.ToUpdateItemResponse();
    }

    public async Task<bool> DeleteItem(int itemId, string userId)
    {
        var item = await dbContext.Items.FirstOrDefaultAsync(i => i.Id == itemId && i.UserId == userId);

        if (item is null)
            return false;

        dbContext.Items.Remove(item);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static (string? MediaType, byte[]? Base64) ParseDataUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return (null, null);

        var components = url.Split(':', ';', ',');

        if (components.Length is < 3 or > 4)
            throw new FormatException(
                $"Invalid data URL. Expected format (;base64 is optional): 'data:[<media-type>][;base64],<data>'. URL given: '{url}'"
            );

        var mediaType = components[1];
        var data = Convert.FromBase64String(components[^1]);

        return (MediaType: mediaType, Base64: data);
    }
}
