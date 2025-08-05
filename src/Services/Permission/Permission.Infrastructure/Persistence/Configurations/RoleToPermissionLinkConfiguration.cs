using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Permission.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence.Configurations
{
    public class RoleToPermissionLinkConfiguration : IEntityTypeConfiguration<RoleToPermissionLink>
    {
        public void Configure(EntityTypeBuilder<RoleToPermissionLink> builder)
        {
            builder.ToTable("Role_Permissions");
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        }
    }
}
