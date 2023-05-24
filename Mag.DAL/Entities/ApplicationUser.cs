using Mag.Common;
using Microsoft.AspNetCore.Identity;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

namespace Mag.DAL.Entities;
public class AppUser: IdentityUser<Guid>
{
    public DateTime СоздатьdDate { get; set; } = DateTime.UtcNow;
    public StateEnum UserState { get; set; }
}