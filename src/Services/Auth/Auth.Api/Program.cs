using Auth.Application;
using Auth.Infrastructure;
using Shared.Application;
using Shared.Infrastructure;
using Shared.AspNetCore;
using Shared.AspNetCore.Extensions;
using Shared.AspNetCore.Middleware;
using Shared.Clients;
using Shared.Options;
using Shared.Security.Extensions;
using Auth.Domain.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Dependency Injection Konfiguration ---
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddSharedApplicationServices()
    .AddSharedInfrastructureServices()
    .AddSharedAspNetCoreServices()
    .AddSharedClients(builder.Configuration);

// Binde die JWT-Optionen aus der appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
    };
});

// Registriert alle Policies aus allen Listen.
var allPermissions = UserPermissions.AllPermissions;

builder.Services.AddPermissionsAuthorization(allPermissions);

builder.Services.Configure<MessageBrokerOptions>(
    builder.Configuration.GetSection(MessageBrokerOptions.SectionName));

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

app.UseMiddleware<GatewayIpCheckMiddleware>();
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