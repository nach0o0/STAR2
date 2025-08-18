using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CostObject.Domain.Entities;

namespace CostObject.Infrastructure.Persistence.Configurations
{
    public class LabelConfiguration : IEntityTypeConfiguration<Label>
    {
        public void Configure(EntityTypeBuilder<Label> builder)
        {
            builder.ToTable("Labels");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name).IsRequired();

            // Stellt sicher, dass pro EmployeeGroup jeder Label-Name nur einmal vorkommt
            builder.HasIndex(l => new { l.EmployeeGroupId, l.Name }).IsUnique();
        }
    }
}
