using Session.Application;
using Session.Infrastructure;
using Shared.Application;
using Shared.AspNetCore;
using Shared.AspNetCore.Extensions;
using Shared.Clients;
using Shared.Infrastructure;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// --- DI Konfiguration ---
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddSharedApplicationServices()
    .AddSharedInfrastructureServices()
    .AddSharedAspNetCoreServices()
    .AddSharedClients(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

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
