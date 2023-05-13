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
    public DbSet<UserState> UserState { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        InitDefaultData();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    private void InitDefaultData()
    {
        if (!UserState.Any())
        {
            var states = new List<UserState>()
            {
                new UserState()
                    { State = StateEnum.Active, Descriptions = "Default state for users", Id = Guid.NewGuid() },
                new UserState() 
                    { State = StateEnum.Blocked, Descriptions = "This user blocked", Id = Guid.NewGuid() },
                new UserState() 
                    { State = StateEnum.Deleted, Descriptions = "Deleted only from the system, not from the database", Id = Guid.NewGuid() }
            };
            UserState.AddRange(states);
        }
        if (!Roles.Any())
        {
            foreach (var role in DefaultRoles.Roles)
            {
                Roles.Add(new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = role.Name });
            }
        }
        SaveChanges();
    }
}