using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shared.Application.Interfaces.Infrastructure;
using Shared.AspNetCore.Middleware;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Application.IntegrationTests.TestUtils
{
    // Wir übergeben eine "Marker"-Klasse aus dem Auth.Api-Projekt, damit der Factory weiß, welches Projekt er starten soll.
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _dbName = $"InMemoryDbForTesting-{Guid.NewGuid()}";

        public Mock<IOrganizationServiceClient> OrganizationServiceClientMock { get; }

        public CustomWebApplicationFactory()
        {
            OrganizationServiceClientMock = new Mock<IOrganizationServiceClient>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            });

            // WICHTIGE ÄNDERUNG: Verwende ConfigureTestServices
            builder.ConfigureTestServices(services =>
            {
                // 1. Bestehende DbContext-Registrierung entfernen und ersetzen
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                // 2. Bestehenden Service-Client entfernen und durch Mock ersetzen
                var clientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IOrganizationServiceClient));
                if (clientDescriptor != null)
                {
                    services.Remove(clientDescriptor);
                }
                services.AddSingleton(OrganizationServiceClientMock.Object);

                // 3. (NEU) GatewayIpCheckMiddleware entfernen/deaktivieren
                // Wir fügen einen leeren "Dummy"-Filter hinzu, der nichts tut,
                // anstatt zu versuchen, die Middleware direkt zu entfernen, was komplizierter sein kann.
                // Oder wir entfernen die Registrierung, falls sie direkt erfolgt.
                var middlewareDescriptor = services.SingleOrDefault(d => d.ImplementationType == typeof(GatewayIpCheckMiddleware));
                if (middlewareDescriptor != null)
                {
                    services.Remove(middlewareDescriptor);
                }
            });
        }

        public string GenerateTestJwtToken(Guid userId, string email, Dictionary<string, List<string>> permissionsByScope)
        {
            using var scope = Services.CreateScope();
            var jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (permissionsByScope.Any())
            {
                var permissionsJson = JsonSerializer.Serialize(permissionsByScope);
                claims.Add(new Claim("permissions_by_scope", permissionsJson, JsonClaimValueTypes.Json));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Token ist 15 Minuten gültig
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
