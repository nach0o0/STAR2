using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence
{
    public class OrganizationDbContextFactory : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            // Konfiguration laden, um an den Connection String zu kommen.
            // Geht 3 Ebenen vom aktuellen Verzeichnis (Infrastructure) nach oben zum Solution-Root
            // und dann ins Api-Projekt, um die appsettings.json zu finden.
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Organization.Api"));
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            var connectionString = configuration.GetConnectionString("OrganizationDbConnection");
            optionsBuilder.UseNpgsql(connectionString);

            // Erstelle ein Mock-Objekt für IPublisher, da es zur Design-Zeit nicht benötigt wird.
            var publisherMock = new Mock<IPublisher>();

            return new OrganizationDbContext(optionsBuilder.Options, publisherMock.Object);
        }
    }
}
