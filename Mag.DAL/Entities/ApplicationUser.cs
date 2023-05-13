﻿using Mag.Common;
using Microsoft.AspNetCore.Identity;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

namespace Mag.DAL.Entities;
public class AppUser: IdentityUser<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public UserState UserState { get; set; }
    public List<IdentityRole> Roles { get; set; }
}