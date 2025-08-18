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
    public class WorkModelConfiguration : IEntityTypeConfiguration<WorkModel>
    {
        public void Configure(EntityTypeBuilder<WorkModel> builder)
        {
            builder.ToTable("WorkModels");
            builder.HasKey(wm => wm.Id);

            builder.Property(wm => wm.Name).IsRequired();

            // Definiert die Präzision für alle Stunden-Dezimalwerte
            var decimalProperties = typeof(WorkModel).GetProperties()
                .Where(p => p.PropertyType == typeof(decimal));

            foreach (var property in decimalProperties)
            {
                builder.Property(property.Name).HasColumnType("decimal(4, 2)");
            }
        }
    }

}
