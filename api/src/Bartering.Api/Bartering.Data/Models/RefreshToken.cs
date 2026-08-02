namespace Bartering.Data.Models;

public class RefreshToken
{
    public required string Token { get; set; }
    public DateTime ExpiresOn { get; set; }

    public required string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
