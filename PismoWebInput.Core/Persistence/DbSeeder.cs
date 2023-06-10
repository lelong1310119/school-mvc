using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PismoWebInput.Core.Enums;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Extensions;
using PismoWebInput.Core.Persistence.Contexts;

namespace PismoWebInput.Core.Persistence;

public static class DbSeeder
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<EfContext>();
        await context.Database.MigrateAsync();
        await IdentityInitialize(serviceProvider);
    }

    public static async Task IdentityInitialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<EfContext>();
        if (context != null)
        {
            var roles = Enum<AppRoleEnum>.ToNameList;

            foreach (var role in roles)
            {
                var roleStore = new RoleStore<AppRole>(context);

                if (!await context.Roles.AnyAsync(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new AppRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }


            var user = new AppUser
            {
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!await context.Users.AnyAsync(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user, "Open4me!");
                user.PasswordHash = hashed;

                var userStore = new UserStore<AppUser>(context);
                await userStore.CreateAsync(user);

            }

            await AssignRoles(serviceProvider, user.Id, roles);

            await context.SaveChangesAsync();
        }
    }

    public static async Task<IdentityResult?> AssignRoles(IServiceProvider services, string userId, List<string> roles)
    {
        var userManager = services.GetService<UserManager<AppUser>>();
        if (userManager == null) return null;
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return null;
        var result = await userManager.AddToRolesAsync(user, roles);
        return result;
    }
}