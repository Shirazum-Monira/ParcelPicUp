using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using parcelPicUp.Models;
using ParcelPicUp.Models;

namespace parcelPicUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<parcelPicUp.Models.Parcel> Parcel { get; set; } = default!;
        public DbSet<ParcelPicUp.Models.ContactConfig> ContactConfig { get; set; } = default!;
    }
}
