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
    public class HierarchyDefinitionConfiguration : IEntityTypeConfiguration<HierarchyDefinition>
    {
        public void Configure(EntityTypeBuilder<HierarchyDefinition> builder)
        {
            builder.ToTable("HierarchyDefinitions");
            builder.HasKey(hd => hd.Id);

            builder.Property(hd => hd.Name).IsRequired();

            // Stellt sicher, dass pro EmployeeGroup jeder Hierarchie-Name nur einmal vorkommt
            builder.HasIndex(hd => new { hd.EmployeeGroupId, hd.Name }).IsUnique();
        }
    }
}
