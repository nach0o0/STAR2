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
    public class UserUnassignedFromEmployeeConsumer : IConsumer<UserUnassignedFromEmployeeIntegrationEvent>
    {
        private readonly IActiveSessionRepository _sessionRepository;

        public UserUnassignedFromEmployeeConsumer(IActiveSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task Consume(ConsumeContext<UserUnassignedFromEmployeeIntegrationEvent> context)
        {
            // Wenn die Zuweisung aufgehoben wird, sind ebenfalls alle Sitzungen veraltet,
            // da der employee_id-Claim nun entfernt werden muss.
            var userId = context.Message.UserId;

            var sessions = await _sessionRepository.GetByUserIdAsync(userId, context.CancellationToken);
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
