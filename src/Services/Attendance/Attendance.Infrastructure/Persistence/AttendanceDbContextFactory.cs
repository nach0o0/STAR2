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

namespace Attendance.Infrastructure.Persistence
{
    public class AttendanceDbContextFactory : IDesignTimeDbContextFactory<AttendanceDbContext>
    {
        public AttendanceDbContext CreateDbContext(string[] args)
        {
            // Konfiguration laden, um an den Connection String zu kommen.
            // Geht 3 Ebenen vom aktuellen Verzeichnis (Infrastructure) nach oben zum Solution-Root
            // und dann ins Api-Projekt, um die appsettings.json zu finden.
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Attendance.Api"));
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AttendanceDbContext>();
            var connectionString = configuration.GetConnectionString("AttendanceDbConnection");
            optionsBuilder.UseNpgsql(connectionString);

            // Erstelle ein Mock-Objekt für IPublisher, da es zur Design-Zeit nicht benötigt wird.
            var publisherMock = new Mock<IPublisher>();

            return new AttendanceDbContext(optionsBuilder.Options, publisherMock.Object);
        }
    }
}
