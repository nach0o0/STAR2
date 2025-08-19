using CostObject.Application.Interfaces.Persistence;
using MassTransit;
using Shared.Application.Interfaces.Persistence;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.IntegrationEventConsumers
{
    public class EmployeeGroupDeletedConsumer : IConsumer<EmployeeGroupDeletedIntegrationEvent>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly IHierarchyDefinitionRepository _hierarchyDefinitionRepository;
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeGroupDeletedConsumer(
            ICostObjectRepository costObjectRepository,
            ILabelRepository labelRepository,
            IHierarchyDefinitionRepository hierarchyDefinitionRepository,
            ICostObjectRequestRepository requestRepository,
            IUnitOfWork unitOfWork)
        {
            _costObjectRepository = costObjectRepository;
            _labelRepository = labelRepository;
            _hierarchyDefinitionRepository = hierarchyDefinitionRepository;
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EmployeeGroupDeletedIntegrationEvent> context)
        {
            var groupId = context.Message.EmployeeGroupId;

            // 1. Alle CostObjectRequests für diese Gruppe löschen
            var requests = await _requestRepository.GetByGroupIdAsync(groupId, context.CancellationToken);
            foreach (var request in requests)
            {
                _requestRepository.Delete(request);
            }

            // 2. Alle CostObjects für diese Gruppe löschen
            var costObjects = await _costObjectRepository.GetByGroupIdAsync(groupId, context.CancellationToken);
            foreach (var costObject in costObjects)
            {
                costObject.PrepareForDeletion();
                _costObjectRepository.Delete(costObject);
            }

            // 3. Alle HierarchyDefinitions (und implizit deren Levels) für diese Gruppe löschen
            var definitions = await _hierarchyDefinitionRepository.GetByGroupIdAsync(groupId, context.CancellationToken);
            foreach (var definition in definitions)
            {
                // Hinweis: Der HierarchyDefinitionDeletedEventHandler kümmert sich um das Löschen der Levels
                definition.PrepareForDeletion();
                _hierarchyDefinitionRepository.Delete(definition);
            }

            // 4. Alle Labels für diese Gruppe löschen
            var labels = await _labelRepository.GetByGroupIdAsync(groupId, context.CancellationToken);
            foreach (var label in labels)
            {
                _labelRepository.Delete(label);
            }

            // Alle Löschoperationen in einer Transaktion speichern
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
