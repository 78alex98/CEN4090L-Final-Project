using System.ComponentModel;

namespace Bartering.Core.Features.Bids.Dtos.Requests;

public record CreateBidRequest([property: Description("The item to place as a bid.")] int ItemId);
