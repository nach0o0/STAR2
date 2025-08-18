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
    public class CostObjectConfiguration : IEntityTypeConfiguration<Domain.Entities.CostObject>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.CostObject> builder)
        {
            builder.ToTable("CostObjects");
            builder.HasKey(co => co.Id);

            builder.Property(co => co.Name).IsRequired();
            builder.Property(co => co.ValidFrom).HasColumnType("date");
            builder.Property(co => co.ValidTo).HasColumnType("date");

            // Konvertiert den Enum in einen String für die Datenbank
            builder.Property(co => co.ApprovalStatus)
                .HasConversion<string>()
                .IsRequired();

            // Definiert die optionale, selbst-referenzierende Beziehung für die Hierarchie
            builder.HasOne<Domain.Entities.CostObject>()
                   .WithMany()
                   .HasForeignKey(co => co.ParentCostObjectId)
                   .OnDelete(DeleteBehavior.Restrict); // Verhindert das Löschen von Eltern-Kostenstellen

            // Beziehung zu HierarchyLevel
            builder.HasOne<HierarchyLevel>()
                   .WithMany()
                   .HasForeignKey(co => co.HierarchyLevelId)
                   .IsRequired();

            // Optionale Beziehung zu Label
            builder.HasOne<Label>()
                   .WithMany()
                   .HasForeignKey(co => co.LabelId)
                   .IsRequired(false);
        }
    }
}
