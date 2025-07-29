using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Auth.Infrastructure.Persistence
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            // Konfiguration laden, um an den Connection String zu kommen.
            // Geht 3 Ebenen vom aktuellen Verzeichnis (Infrastructure) nach oben zum Solution-Root
            // und dann ins Api-Projekt, um die appsettings.json zu finden.
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Auth.Api"));
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
            var connectionString = configuration.GetConnectionString("AuthDbConnection");
            optionsBuilder.UseNpgsql(connectionString);

            // Erstelle ein Mock-Objekt für IPublisher, da es zur Design-Zeit nicht benötigt wird.
            var publisherMock = new Mock<IPublisher>();

            return new AuthDbContext(optionsBuilder.Options, publisherMock.Object);
        }
    }
}
