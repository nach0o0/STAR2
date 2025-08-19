using MassTransit;
using Planning.Application.Interfaces.Persistence;
using Shared.Messages.Events.CostObjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.IntegrationEventConsumers
{
    public class CostObjectDeletedConsumer : IConsumer<CostObjectDeletedIntegrationEvent>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;

        public CostObjectDeletedConsumer(IPlanningEntryRepository planningEntryRepository)
        {
            _planningEntryRepository = planningEntryRepository;
        }

        public async Task Consume(ConsumeContext<CostObjectDeletedIntegrationEvent> context)
        {
            await _planningEntryRepository.DeleteByCostObjectIdAsync(context.Message.CostObjectId, context.CancellationToken);
        }
    }
}
