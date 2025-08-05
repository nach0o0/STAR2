using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Session.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Infrastructure.Persistence.Configurations
{
    public class ActiveSessionConfiguration : IEntityTypeConfiguration<ActiveSession>
    {
        public void Configure(EntityTypeBuilder<ActiveSession> builder)
        {
            builder.ToTable("ActiveSessions");

            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.VerifierHash).IsUnique();

            builder.Property(s => s.VerifierHash)
                .IsRequired();
        }
    }
}
