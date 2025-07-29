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
    public class HourlyRateConfiguration : IEntityTypeConfiguration<HourlyRate>
    {
        public void Configure(EntityTypeBuilder<HourlyRate> builder)
        {
            builder.ToTable("HourlyRates");
            builder.HasKey(hr => hr.Id);
            builder.Property(hr => hr.Rate).HasColumnType("decimal(18,2)");
        }
    }
}
