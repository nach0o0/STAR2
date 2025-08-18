using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Persistence.Configurations
{
    public class PublicHolidayConfiguration : IEntityTypeConfiguration<PublicHoliday>
    {
        public void Configure(EntityTypeBuilder<PublicHoliday> builder)
        {
            builder.ToTable("PublicHolidays");
            builder.HasKey(ph => ph.Id);

            builder.Property(ph => ph.Name).IsRequired();
            builder.Property(ph => ph.Date).HasColumnType("date");

            // Verhindert doppelte Feiertage pro Gruppe am selben Tag
            builder.HasIndex(ph => new { ph.EmployeeGroupId, ph.Date }).IsUnique();
        }
    }
}
