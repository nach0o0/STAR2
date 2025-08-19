using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Infrastructure.Persistence.Configurations
{
    public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            builder.ToTable("TimeEntries");
            builder.HasKey(te => te.Id);

            builder.Property(te => te.EntryDate).HasColumnType("date");

            builder.Property(te => te.Hours).HasColumnType("decimal(5, 2)").IsRequired();
            builder.Property(te => te.HourlyRate).HasColumnType("decimal(18, 2)").IsRequired();

            // Indizes verbessern die Abfrageperformance erheblich.
            builder.HasIndex(te => te.EmployeeId);
            builder.HasIndex(te => te.CostObjectId);
            builder.HasIndex(te => te.EmployeeGroupId);
            builder.HasIndex(te => te.AccessKey).IsUnique();
        }
    }
}
