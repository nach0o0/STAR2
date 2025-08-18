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
    public class AttendanceEntryConfiguration : IEntityTypeConfiguration<AttendanceEntry>
    {
        public void Configure(EntityTypeBuilder<AttendanceEntry> builder)
        {
            builder.ToTable("AttendanceEntries");
            builder.HasKey(ae => ae.Id);

            // Stellt sicher, dass das Datum als reines Datum ohne Zeit gespeichert wird
            builder.Property(ae => ae.Date).HasColumnType("date");

            // Ein Mitarbeiter kann pro Tag nur einen Eintrag haben
            builder.HasIndex(ae => new { ae.EmployeeId, ae.Date }).IsUnique();

            // Hinzugefügte Fremdschlüssel-Beziehung
            builder.HasOne(e => e.AttendanceType)
                   .WithMany() // Ein Typ kann viele Einträge haben
                   .HasForeignKey(e => e.AttendanceTypeId)
                   .IsRequired();
        }
    }
}
