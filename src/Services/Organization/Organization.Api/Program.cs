using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Organization.Application;
using Organization.Domain.Authorization;
using Organization.Infrastructure;
using Organization.Infrastructure.Persistence;
using Shared.Application;
using Shared.Application.Behaviors;
using Shared.AspNetCore;
using Shared.AspNetCore.Extensions;
using Shared.AspNetCore.Middleware;
using Shared.Clients;
using Shared.Infrastructure;
using Shared.Options;
using Shared.Security;
using Shared.Security.Authorization;
using Shared.Security.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- DI Konfiguration ---
builder.Services.AddHttpContextAccessor();

var organizationApplicationAssembly = typeof(Organization.Application.DependencyInjection).Assembly;
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddSharedApplicationServices(organizationApplicationAssembly)
    .AddSharedAspNetCoreServices()
    .AddSharedInfrastructureServices()
    .AddSharedClients(builder.Configuration);

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

var allPermissions = OrganizationPermissions.AllPermissions
    .Concat(EmployeeGroupPermissions.AllPermissions)
    .Concat(EmployeePermissions.AllPermissions)
    .Concat(HourlyRatePermissions.AllPermissions)
    .Concat(InvitationPermissions.AllPermissions);

// Registriert alle Policies aus allen Listen.
builder.Services.AddPermissionsAuthorization(allPermissions);

builder.Services.Configure<MessageBrokerOptions>(
    builder.Configuration.GetSection(MessageBrokerOptions.SectionName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.RegisterServicePermissionsAsync("Organization", allPermissions);

// --- HTTP Request Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GatewayIpCheckMiddleware>();
app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();