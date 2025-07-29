using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Domain.Entities.Organization>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Organization> builder)
        {
            builder.ToTable("Organizations");
            builder.HasKey(o => o.Id);
        }
    }
}
