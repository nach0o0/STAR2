using MassTransit;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Persistence;
using Shared.Messages.Events.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.IntegrationEventConsumers
{
    public class UserAccountDeletedConsumer : IConsumer<UserAccountDeletedIntegrationEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserAccountDeletedConsumer(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserAccountDeletedIntegrationEvent> context)
        {
            var userId = context.Message.UserId;

            // Finde den Mitarbeiter, der mit dem gelöschten Benutzerkonto verknüpft ist.
            var employee = await _employeeRepository.GetByUserIdAsync(userId, context.CancellationToken);
            if (employee is null)
            {
                return; // Kein Mitarbeiterprofil für diesen Benutzer gefunden, nichts zu tun.
            }

            // Hebe die Verknüpfung auf.
            employee.UnassignUser();

            // Speichere die Änderung.
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
