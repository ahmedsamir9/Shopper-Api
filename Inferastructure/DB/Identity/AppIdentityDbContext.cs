using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inferastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().Property(p => p.FullName).IsRequired().HasMaxLength(50);
            builder.Entity<AppUser>().OwnsOne(a => a.Address, o =>
              o.WithOwner());
            base.OnModelCreating(builder);
        }
    }
}
