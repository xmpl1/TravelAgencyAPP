using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPP.Models.Data;

namespace TravelAgencyAPP.Models
{
    public class AppCtx : IdentityDbContext<User>
    {
        public AppCtx(DbContextOptions<AppCtx> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users{ get; set; }
        public DbSet<Country> Countries{ get; set; }
        public DbSet<Hotel> Hotels{ get; set; }
        public DbSet<Tour> Tours { get; set; }
    }
}
