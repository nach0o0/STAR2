using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence.Configurations
{
    public class UserPermissionAssignmentConfiguration : IEntityTypeConfiguration<UserPermissionAssignment>
    {
        public void Configure(EntityTypeBuilder<UserPermissionAssignment> builder)
        {
            builder.ToTable("UserPermissionAssignments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Scope).IsRequired();
        }
    }
}
