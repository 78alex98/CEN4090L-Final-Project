using Bartering.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Data.Database;

public class BarteringDbContext(DbContextOptions<BarteringDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<ItemIsListed> ItemIsListed { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This needs to be called first to ensure that the configurations defined below override.
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<ApplicationUser>()
            .ToTable("users")
            .Property(u => u.RegistrationDate)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<RefreshToken>().HasKey(r => r.Token);

        modelBuilder.Entity<Item>().Property(i => i.ImageType).HasComment("MIME Type");

        modelBuilder.Entity<Listing>().Property(l => l.PostedDate).HasDefaultValueSql("now()");
        modelBuilder.Entity<Listing>().Property(l => l.IsOpen).HasDefaultValue(false);

        modelBuilder.Entity<Bid>().Property(b => b.PostedDate).HasDefaultValueSql("now()");

        modelBuilder.Entity<ItemIsListed>().ToView("v_item_is_listed").HasKey(i => i.Id);

        // This FK relation needs to be configured here to work around a circular relation error since Listing and Bid reference each other.
        modelBuilder
            .Entity<Listing>()
            .HasOne(e => e.WinningBid)
            .WithOne()
            .HasForeignKey<Listing>(e => e.WinningBidId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityRole>().ToTable("roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
    }
}
