using MassTransit;
using Planning.Application.Interfaces.Persistence;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.IntegrationEventConsumers
{
    public class EmployeeGroupDeletedConsumer : IConsumer<EmployeeGroupDeletedIntegrationEvent>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;

        public EmployeeGroupDeletedConsumer(IPlanningEntryRepository planningEntryRepository)
        {
            _planningEntryRepository = planningEntryRepository;
        }

        public async Task Consume(ConsumeContext<EmployeeGroupDeletedIntegrationEvent> context)
        {
            await _planningEntryRepository.DeleteByGroupIdAsync(context.Message.EmployeeGroupId, context.CancellationToken);
        }
    }
}
