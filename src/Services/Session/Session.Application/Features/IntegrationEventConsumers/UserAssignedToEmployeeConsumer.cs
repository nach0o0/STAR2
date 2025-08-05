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
    public class UserAssignedToEmployeeConsumer : IConsumer<UserAssignedToEmployeeIntegrationEvent>
    {
        private readonly IActiveSessionRepository _sessionRepository;

        public UserAssignedToEmployeeConsumer(IActiveSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task Consume(ConsumeContext<UserAssignedToEmployeeIntegrationEvent> context)
        {
            // Wenn ein User einem Employee zugewiesen wird, sind alle bestehenden Sitzungen
            // (die noch keine EmployeeId haben) veraltet. Sie müssen gelöscht werden,
            // um ein Update des Tokens zu erzwingen.
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
