using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Permission.Domain.Authorization;
using Permission.Domain.Constants;
using Permission.Domain.Entities;
using Permission.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PermissionDbContext>();

            await SeedRolesAsync(dbContext);
            await dbContext.SaveChangesAsync();
            await SeedRolePermissionsAsync(dbContext);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(PermissionDbContext dbContext)
        {
            if (await dbContext.Roles.AnyAsync())
            {
                return;
            }
            // Füge die vordefinierten Rollen-Objekte hinzu.
            await dbContext.Roles.AddRangeAsync(DefaultRoleDefinitions.All);
        }

        private static async Task SeedRolePermissionsAsync(PermissionDbContext dbContext)
        {
            if (await dbContext.RolePermissions.AnyAsync())
            {
                return;
            }

            // 2. Verwende die IDs der statischen Objekte direkt.
            var rolePermissions = new List<RoleToPermissionLink>
            {
                // Weise dem System-Admin alle Berechtigungen aus dem PermissionService zu.
                new(DefaultRoleDefinitions.SystemAdminRole.Id, PermissionPermissions.Read),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, RolePermissions.Read),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, RolePermissions.Create),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, RolePermissions.Update),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, RolePermissions.Delete),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, RolePermissions.AssignPermission),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, AssignmentPermissions.ReadAssignments),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, AssignmentPermissions.AssignRole),
                new(DefaultRoleDefinitions.SystemAdminRole.Id, AssignmentPermissions.AssignDirect)
            };

            await dbContext.RolePermissions.AddRangeAsync(rolePermissions);
        }
    }
}
