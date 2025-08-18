using CostObject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Configurations
{
    public class HierarchyLevelConfiguration : IEntityTypeConfiguration<HierarchyLevel>
    {
        public void Configure(EntityTypeBuilder<HierarchyLevel> builder)
        {
            builder.ToTable("HierarchyLevels");
            builder.HasKey(hl => hl.Id);

            builder.Property(hl => hl.Name).IsRequired();

            // Beziehung zu HierarchyDefinition
            builder.HasOne<HierarchyDefinition>()
                   .WithMany()
                   .HasForeignKey(hl => hl.HierarchyDefinitionId)
                   .IsRequired();
        }
    }
}
