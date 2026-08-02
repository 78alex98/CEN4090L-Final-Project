using Bartering.Core.Features.Items.Dtos.Requests;
using Bartering.Core.Features.Items.Dtos.Responses;

namespace Bartering.Core.Features.Items;

public interface IItemService
{
    Task<GetItemResponse?> GetItem(int itemId, string userId);

    Task<IEnumerable<GetInventoryResponse>> GetInventory(string userId);

    Task<CreateItemResponse> CreateItem(CreateItemRequest request, string userId);

    Task<UpdateItemResponse?> UpdateItem(UpdateItemRequest request, string userId);

    Task<bool> DeleteItem(int itemId, string userId);
}
