using Auth.Application;
using Auth.Infrastructure;
using Shared.Application;
using Shared.Infrastructure;
using Shared.AspNetCore;
using Shared.AspNetCore.Extensions;
using Shared.AspNetCore.Middleware;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Dependency Injection Konfiguration ---
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddSharedApplicationServices()
    .AddSharedInfrastructureServices()
    .AddSharedAspNetCoreServices();

// Binde die JWT-Optionen aus der appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

// Füge die Controller-Dienste hinzu
builder.Services.AddControllers();

// Füge Swagger/OpenAPI für die API-Dokumentation hinzu
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ---------------------------------------------


var app = builder.Build();

// --- 2. HTTP Request Pipeline Konfiguration ---
// Aktiviere Swagger UI nur in der Entwicklungsumgebung
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Füge die globale Fehlerbehandlungs-Middleware hinzu
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

// Authentifizierung und Autorisierung aktivieren
// Wichtig: app.UseAuthentication() MUSS VOR app.UseAuthorization() stehen.
app.UseAuthentication();
app.UseAuthorization();

// Mappt die Controller-Routen
app.MapControllers();
// ---------------------------------------------

app.Run();