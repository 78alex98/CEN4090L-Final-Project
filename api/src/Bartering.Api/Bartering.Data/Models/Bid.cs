namespace Bartering.Data.Models;

public class Bid
{
    public int Id { get; set; }
    public DateTime PostedDate { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
}
