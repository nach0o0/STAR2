using CostObject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Configurations
{
    public class CostObjectRequestConfiguration : IEntityTypeConfiguration<CostObjectRequest>
    {
        public void Configure(EntityTypeBuilder<CostObjectRequest> builder)
        {
            builder.ToTable("CostObjectRequests");
            builder.HasKey(cor => cor.Id);

            // Konvertiert den Enum in einen String
            builder.Property(cor => cor.Status)
                .HasConversion<string>()
                .IsRequired();

            // Beziehung zur Kostenstelle, auf die sich der Antrag bezieht
            builder.HasOne<Domain.Entities.CostObject>()
                   .WithMany()
                   .HasForeignKey(cor => cor.CostObjectId)
                   .IsRequired();
        }
    }
}
