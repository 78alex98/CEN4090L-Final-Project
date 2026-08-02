namespace Bartering.Data.Models;

public class Listing
{
    public int Id { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime? ClosedDate { get; set; } = null;
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Message { get; set; }
    public bool IsOpen { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public int? WinningBidId { get; set; } = null;
    public Bid WinningBid { get; set; } = null!;
}
