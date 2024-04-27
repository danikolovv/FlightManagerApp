using FlightManagerApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlightManagerApp.Models;
using System.Data;

namespace FlightManagerApp.Data;

public class FlightManagerContext : IdentityDbContext<ApplicationUser>
{
    public FlightManagerContext(DbContextOptions<FlightManagerContext> options)
        : base(options)
    {
    }

    public DbSet<Flight> Flight { get; set; }

    public DbSet<Reservation> Reservation { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<FlightManagerApp.Models.AdminModel> AdminModel { get; set; } = default!;

}
