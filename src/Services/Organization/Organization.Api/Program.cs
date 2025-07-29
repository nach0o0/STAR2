using MediatR;
using Microsoft.AspNetCore.Authorization;
using Organization.Application;
using Organization.Domain.Authorization;
using Organization.Infrastructure;
using Organization.Infrastructure.Persistence;
using Shared.Application;
using Shared.Application.Behaviors;
using Shared.AspNetCore;
using Shared.AspNetCore.Extensions;
using Shared.Clients;
using Shared.Infrastructure;
using Shared.Options;
using Shared.Security;
using Shared.Security.Authorization;
using Shared.Security.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- DI Konfiguration ---
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddSharedApplicationServices()
    .AddSharedAspNetCoreServices()
    .AddSharedInfrastructureServices()
    .AddSharedClients(builder.Configuration);

// TODO: Authentifizierung (JWT Bearer) hinzufügen


var allPermissions = OrganizationPermissions.AllPermissions;
    //.Concat(EmployeePermissions.AllPermissions)
    //.Concat(EmployeeGroupPermissions.AllPermissions);

// Registriert alle Policies aus allen Listen.
builder.Services.AddPermissionsAuthorization(allPermissions);

builder.Services.Configure<MessageBrokerOptions>(
    builder.Configuration.GetSection(MessageBrokerOptions.SectionName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- HTTP Request Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();