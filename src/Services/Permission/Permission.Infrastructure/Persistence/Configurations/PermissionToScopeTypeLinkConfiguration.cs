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
    public class PermissionToScopeTypeLinkConfiguration : IEntityTypeConfiguration<PermissionToScopeTypeLink>
    {
        public void Configure(EntityTypeBuilder<PermissionToScopeTypeLink> builder)
        {
            builder.ToTable("Permission_PermittedScopeTypes");

            // Definiert den zusammengesetzten Primärschlüssel.
            builder.HasKey(l => new { l.PermissionId, l.ScopeType });
        }
    }
}
