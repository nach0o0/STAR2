using Attendance.Application.Features.IntegrationEventConsumers;
using Attendance.Application.Interfaces.Persistence;
using Attendance.Infrastructure.Persistence;
using Attendance.Infrastructure.Persistence.Repositories;
using Attendance.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;

namespace Attendance.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AttendanceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AttendanceDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AttendanceDbContext>());
            services.AddScoped<IAttendanceEntryRepository, AttendanceEntryRepository>();
            services.AddScoped<IAttendanceTypeRepository, AttendanceTypeRepository>();
            services.AddScoped<IEmployeeWorkModelRepository, EmployeeWorkModelRepository>();
            services.AddScoped<IPublicHolidayRepository, PublicHolidayRepository>();
            services.AddScoped<IWorkModelRepository, WorkModelRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<EmployeeEmployeeGroupAssignmentChangedIntegrationEventConsumer>();
                busConfigurator.AddConsumer<EmployeeGroupDeletedIntegrationEventConsumer>();

                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    // Hole die stark typisierten Optionen aus dem DI-Container
                    var options = context.GetRequiredService<IOptions<MessageBrokerOptions>>().Value;

                    cfg.Host(options.Host, "/", h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
