using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Planning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Infrastructure.Persistence.Configurations
{
    public class PlanningEntryConfiguration : IEntityTypeConfiguration<PlanningEntry>
    {
        public void Configure(EntityTypeBuilder<PlanningEntry> builder)
        {
            builder.ToTable("PlanningEntries");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PlannedHours)
                .HasColumnType("decimal(5, 2)")
                .IsRequired();

            builder.Property(p => p.PlanningPeriodStart)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(p => p.PlanningPeriodEnd)
                .HasColumnType("date")
                .IsRequired();

            // Indizes verbessern die Abfrageperformance für häufig gefilterte Spalten.
            builder.HasIndex(p => p.EmployeeId);
            builder.HasIndex(p => p.CostObjectId);
            builder.HasIndex(p => p.EmployeeGroupId);
        }
    }
}
