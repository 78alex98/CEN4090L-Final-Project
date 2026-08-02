using Microsoft.AspNetCore.Identity;

namespace Bartering.Data.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime RegistrationDate { get; set; }
}
