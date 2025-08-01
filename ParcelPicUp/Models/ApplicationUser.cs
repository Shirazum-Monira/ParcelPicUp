using Microsoft.AspNetCore.Identity;

namespace parcelPicUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; } 

    }
}
