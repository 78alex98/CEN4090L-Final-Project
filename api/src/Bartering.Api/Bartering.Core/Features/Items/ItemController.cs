using Bartering.Core.Common.Util;
using Bartering.Core.Features.Items.Dtos.Requests;
using Bartering.Core.Features.Items.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bartering.Core.Features.Items;

[Authorize]
[ApiController]
[Tags("Items")]
[Route("api/[controller]s")]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;

    private readonly IItemService _service;

    public ItemController(ILogger<ItemController> logger, IItemService service)
    {
        _logger = logger;
        _service = service;
    }

    [EndpointSummary("Get an item")]
    [EndpointDescription("Gets a specified item.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetItemResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetItem(int itemId)
    {
        var accessToken = Request.Cookies["accessToken"];

        if (accessToken is null)
        {
            return Unauthorized();
        }

        var userId = AccessTokenUtility.GetUserId(accessToken);

        if (string.IsNullOrEmpty(userId))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "The access token is missing the user's id.");
        }

        var result = await _service.GetItem(itemId, userId);

        return result is not null ? Ok(result) : NotFound();
    }

    [EndpointSummary("Get the inventory of the authenticated user")]
    [EndpointDescription("Gets a list of the authenticated user's items.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInventoryResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpGet("/api/inventory")]
    public async Task<IActionResult> GetInventory()
    {
        var accessToken = Request.Cookies["accessToken"];

        if (accessToken is null)
        {
            return Unauthorized();
        }

        var id = AccessTokenUtility.GetUserId(accessToken);

        if (string.IsNullOrEmpty(id))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "The access token is missing the user's id.");
        }

        var result = await _service.GetInventory(id);

        return Ok(result);
    }

    [EndpointSummary("Create an item")]
    [EndpointDescription(
        "Creates an item for the authenticated user. The image must be a data URL with a base64 encoded image."
    )]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateItemResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
    {
        var accessToken = Request.Cookies["accessToken"];

        if (accessToken is null)
        {
            return Unauthorized();
        }

        var userId = AccessTokenUtility.GetUserId(accessToken);

        if (string.IsNullOrEmpty(userId))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "The access token is missing the user's id.");
        }

        var result = await _service.CreateItem(request, userId);

        return Created(string.Empty, result);
    }

    [EndpointSummary("Update an item")]
    [EndpointDescription(
        "Updates an item's data. To update the image, `image` must be a data URL with a base64 encoded image."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateItemResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpPut]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateItemRequest request)
    {
        var accessToken = Request.Cookies["accessToken"];

        if (accessToken is null)
        {
            return Unauthorized();
        }

        var userId = AccessTokenUtility.GetUserId(accessToken);

        if (string.IsNullOrEmpty(userId))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "The access token is missing the user's id.");
        }

        var result = await _service.UpdateItem(request, userId);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [EndpointSummary("Delete an item")]
    [EndpointDescription(
        "Deletes a specified item. The response contains a message indicating success or failure and, if successful, the deleted item's ID."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        var accessToken = Request.Cookies["accessToken"];

        if (accessToken is null)
        {
            return Unauthorized();
        }

        var userId = AccessTokenUtility.GetUserId(accessToken);

        if (string.IsNullOrEmpty(userId))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "The access token is missing the user's id.");
        }

        var deleted = await _service.DeleteItem(itemId, userId);

        if (!deleted)
        {
            return NotFound(new { message = $"Item with ID {itemId} not found." });
        }

        return Ok(new { message = "Item deleted successfully", deletedItemId = itemId });
    }
}
