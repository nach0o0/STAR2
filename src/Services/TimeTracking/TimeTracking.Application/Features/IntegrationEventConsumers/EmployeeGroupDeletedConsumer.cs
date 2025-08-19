using MassTransit;
using Shared.Application.Interfaces.Persistence;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.IntegrationEventConsumers
{
    public class EmployeeGroupDeletedConsumer : IConsumer<EmployeeGroupDeletedIntegrationEvent>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeGroupDeletedConsumer(ITimeEntryRepository timeEntryRepository, IUnitOfWork unitOfWork)
        {
            _timeEntryRepository = timeEntryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EmployeeGroupDeletedIntegrationEvent> context)
        {
            var groupId = context.Message.EmployeeGroupId;

            await _timeEntryRepository.DeleteByGroupIdAsync(groupId, context.CancellationToken);

            // Da ExecuteDeleteAsync die Änderungen direkt ausführt, ist ein SaveChanges hier
            // technisch nicht mehr nötig, schadet aber auch nicht, um den UoW-Pattern konsistent zu halten.
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
