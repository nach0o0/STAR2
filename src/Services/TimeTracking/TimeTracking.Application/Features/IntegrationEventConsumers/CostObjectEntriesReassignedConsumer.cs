using MassTransit;
using Shared.Messages.Events.CostObjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.IntegrationEventConsumers
{
    public class CostObjectEntriesReassignedConsumer : IConsumer<CostObjectEntriesReassignedIntegrationEvent>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public CostObjectEntriesReassignedConsumer(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task Consume(ConsumeContext<CostObjectEntriesReassignedIntegrationEvent> context)
        {
            var message = context.Message;
            await _timeEntryRepository.BulkUpdateCostObjectIdAsync(
                message.SourceCostObjectId,
                message.DestinationCostObjectId,
                context.CancellationToken
            );
        }
    }
}
