using MassTransit;
using Session.Application.Interfaces.Persistence;
using Shared.Messages.Events.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.IntegrationEventConsumers
{
    public class UserAccountDeletedConsumer : IConsumer<UserAccountDeletedIntegrationEvent>
    {
        private readonly IActiveSessionRepository _sessionRepository;

        public UserAccountDeletedConsumer(IActiveSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task Consume(ConsumeContext<UserAccountDeletedIntegrationEvent> context)
        {
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
