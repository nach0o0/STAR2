using MassTransit;
using Session.Application.Interfaces.Persistence;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.IntegrationEventConsumers
{
    public class EmployeeMembershipChangedConsumer :
        IConsumer<EmployeeOrganizationAssignmentChangedIntegrationEvent>,
        IConsumer<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent>
    {
        private readonly IActiveSessionRepository _sessionRepository;

        public EmployeeMembershipChangedConsumer(IActiveSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        // Dieser Handler wird aufgerufen, wenn sich die Organisations-Zuweisung ändert.
        public Task Consume(ConsumeContext<EmployeeOrganizationAssignmentChangedIntegrationEvent> context)
        {
            // Ruft die zentrale Logik mit der UserId aus dem Event auf.
            return InvalidateUserSessions(context.Message.UserId, context.CancellationToken);
        }

        // Dieser Handler wird aufgerufen, wenn sich die Gruppen-Zuweisung ändert.
        public Task Consume(ConsumeContext<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent> context)
        {
            // Ruft ebenfalls die zentrale Logik auf.
            return InvalidateUserSessions(context.Message.UserId, context.CancellationToken);
        }

        // Zentrale Methode, um Code-Duplizierung zu vermeiden.
        private async Task InvalidateUserSessions(Guid userId, CancellationToken cancellationToken)
        {
            var sessions = await _sessionRepository.GetByUserIdAsync(userId, cancellationToken);
            if (!sessions.Any())
            {
                return;
            }

            foreach (var session in sessions)
            {
                _sessionRepository.Delete(session);
            }
        }
    }
}
