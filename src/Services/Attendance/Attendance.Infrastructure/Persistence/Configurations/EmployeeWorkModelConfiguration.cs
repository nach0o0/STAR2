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
    public class EmployeeWorkModelConfiguration : IEntityTypeConfiguration<EmployeeWorkModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkModel> builder)
        {
            builder.ToTable("EmployeeWorkModels");
            builder.HasKey(ewm => ewm.Id);

            // Indizes können die Abfrageleistung verbessern
            builder.HasIndex(ewm => ewm.EmployeeId);
            builder.HasIndex(ewm => new { ewm.EmployeeId, ewm.ValidFrom, ewm.ValidTo });
        }
    }
}
