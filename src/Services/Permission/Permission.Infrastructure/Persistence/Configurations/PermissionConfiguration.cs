using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Domain.Entities.Permission>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.PermittedScopeTypes)
                .WithOne()
                .HasForeignKey(l => l.PermissionId);
        }
    }
}
