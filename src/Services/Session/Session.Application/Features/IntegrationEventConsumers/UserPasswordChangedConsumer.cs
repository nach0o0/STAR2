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
    public class UserPasswordChangedConsumer : IConsumer<UserPasswordChangedIntegrationEvent>
    {
        private readonly IActiveSessionRepository _sessionRepository;

        public UserPasswordChangedConsumer(IActiveSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task Consume(ConsumeContext<UserPasswordChangedIntegrationEvent> context)
        {
            var userId = context.Message.UserId;

            // Finde alle aktiven Sitzungen für den Benutzer.
            var sessions = await _sessionRepository.GetByUserIdAsync(userId, context.CancellationToken);
            if (!sessions.Any())
            {
                return;
            }

            // Lösche jede gefundene Sitzung.
            foreach (var session in sessions)
            {
                _sessionRepository.Delete(session);
            }
        }
    }
}
