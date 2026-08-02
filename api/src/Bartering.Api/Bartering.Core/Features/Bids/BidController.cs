using Bartering.Core.Common.Util;
using Bartering.Core.Features.Bids.Dtos.Requests;
using Bartering.Core.Features.Bids.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bartering.Core.Features.Bids;

[Authorize]
[ApiController]
[Tags("Bids")]
[Route("api/listings/{listingId}/[controller]s")]
public class BidController : ControllerBase
{
    private readonly ILogger<BidController> _logger;

    private readonly IBidService _service;

    public BidController(ILogger<BidController> logger, IBidService service)
    {
        _logger = logger;
        _service = service;
    }

    [EndpointSummary("Get a bid")]
    [EndpointDescription("Gets a specified bid.")]
    [HttpGet("{bidId}")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetBid(int bidId)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [EndpointSummary("Create a bid")]
    [EndpointDescription("Places a bid of an item on a listing.")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateBidResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> CreateBid(int listingId, [FromBody] CreateBidRequest request)
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

        var result = await _service.CreateBid(request, listingId, userId);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, "The item was not found.");
        }

        return Created(string.Empty, result);
    }

    [EndpointSummary("Delete a bid")]
    [EndpointDescription("Deletes a specified bid.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpDelete("{bidId}")]
    public async Task<IActionResult> DeleteBid(int listingId, int bidId)
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

        var deleted = await _service.DeleteBid(listingId, bidId, userId);

        if (!deleted)
        {
            return NotFound(new { message = $"Bid with ID {bidId} not found." });
        }

        return NoContent();
    }

    [EndpointSummary("Select a winning bid")]
    [EndpointDescription("Selects a bid as the winning bid of an open listing. " +
                         "This will delete the listing and swap ownership of bid item and the listed item.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("winningBid/{bidId}")]
    public async Task<IActionResult> SelectWinningBid(int listingId, int bidId)
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

        var successful = await _service.SelectWinningBid(listingId, bidId, userId);

        if (!successful)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return NoContent();
    }
}
