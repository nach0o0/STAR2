using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);

            // Konfiguriert die "Owned Entity" für die Verknüpfungstabelle
            builder.OwnsMany(e => e.EmployeeGroupLinks, linkBuilder =>
            {
                linkBuilder.ToTable("Employee_EmployeeGroup");
                linkBuilder.HasKey("EmployeeId", "EmployeeGroupId");
            });
        }
    }
}
