namespace Bartering.Data.Models;

public class ItemIsListed
{
    public int Id { get; protected set; }
    public string Name { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;

    public byte[]? Image { get; protected set; }

    public string? ImageType { get; protected set; }

    public bool IsListed { get; protected set; }

    public string UserId { get; protected set; } = null!;
    public ApplicationUser User { get; protected set; } = null!;
}
