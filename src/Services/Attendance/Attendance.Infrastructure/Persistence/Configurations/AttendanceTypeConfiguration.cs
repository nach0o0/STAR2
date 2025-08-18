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
    public class AttendanceTypeConfiguration : IEntityTypeConfiguration<AttendanceType>
    {
        public void Configure(EntityTypeBuilder<AttendanceType> builder)
        {
            builder.ToTable("AttendanceTypes");
            builder.HasKey(at => at.Id);

            builder.Property(at => at.Name).IsRequired();
            builder.Property(at => at.Abbreviation).IsRequired();
            builder.Property(at => at.Color).IsRequired();
        }
    }
}
