using Bartering.Core.Common.Util;
using Bartering.Core.Features.Listings.Dtos.Requests;
using Bartering.Core.Features.Listings.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bartering.Core.Features.Listings;

[Authorize]
[ApiController]
[Tags("Listings")]
[Route("api/[controller]s")]
public class ListingController : ControllerBase
{
    private readonly ILogger<ListingController> _logger;

    private readonly IListingService _service;

    public ListingController(ILogger<ListingController> logger, IListingService service)
    {
        _logger = logger;
        _service = service;
    }

    [EndpointSummary("List listings")]
    [EndpointDescription("Gets all listings sorted by most recently created to least recently created.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ListListingsResponse>))]
    [HttpGet]
    public async Task<IActionResult> GetAllListings()
    {
        var result = await _service.ListListings();

        return Ok(result);
    }

    [EndpointSummary("Get a listing")]
    [EndpointDescription("Gets a specified listing.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetListingResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{listingId}")]
    public async Task<IActionResult> GetListing(int listingId)
    {
        var result = await _service.GetListing(listingId);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, "The listing was not found.");
        }

        return Ok(result);
    }

    [EndpointSummary("Create a listing")]
    [EndpointDescription("Creates a listing of an item. The item must not already be listed.")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateListingResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> CreateListing([FromBody] CreateListingRequest request)
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

        var result = await _service.CreateListing(request, userId);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, "The item was not found.");
        }

        return Created(string.Empty, result);
    }

    [EndpointSummary("Update a listing")]
    [EndpointDescription(
        "Updates a listing's data. If the listing is currently closed and is being re-opened, then the winning bid and closed date will be unset."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateListingResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpPatch("{listingId}")]
    public async Task<IActionResult> UpdateListing(int listingId, [FromBody] UpdateListingRequest request)
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

        var result = await _service.UpdateListing(request, listingId, userId);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, "The listing was not found.");
        }

        return Ok(result);
    }

    [EndpointSummary("Delete a listing")]
    [EndpointDescription("Deletes a specified listing and all of its bids.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpDelete("{listingId}")]
    public async Task<IActionResult> DeleteListing(int listingId)
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

        var deleted = await _service.DeleteListing(listingId, userId);

        if (!deleted)
        {
            return NotFound(new { message = $"Listing with ID {listingId} not found." });
        }

        return NoContent();
    }
}
