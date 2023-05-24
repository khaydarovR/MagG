using Mag.Common;
using Mag.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mag.DAL;

public class AppDbContext: IdentityDbContext<AppUser, IdentityRole<Guid>, Guid,
                                            IdentityUserClaim<Guid>, 
                                            IdentityUserRole<Guid>, 
                                            IdentityUserLogin<Guid>, 
                                            IdentityRoleClaim<Guid>, 
                                            IdentityUserToken<Guid>>
{
    public DbSet<AppUser> AppUser { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<NomType> NomTypes { get; set; }
    public DbSet<Nom> Noms { get; set; }
    public DbSet<Order> Orders { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}