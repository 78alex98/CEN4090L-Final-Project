namespace Bartering.Data.Models;

public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public byte[]? Image { get; set; }

    public string? ImageType { get; set; }

    public required string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
