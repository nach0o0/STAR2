using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteHierarchyDefinition
{
    public class DeleteHierarchyDefinitionCommandHandler : IRequestHandler<DeleteHierarchyDefinitionCommand>
    {
        private readonly IHierarchyDefinitionRepository _hierarchyDefinitionRepository;

        public DeleteHierarchyDefinitionCommandHandler(IHierarchyDefinitionRepository hierarchyDefinitionRepository)
        {
            _hierarchyDefinitionRepository = hierarchyDefinitionRepository;
        }

        public async Task Handle(DeleteHierarchyDefinitionCommand command, CancellationToken cancellationToken)
        {
            var hierarchyDefinition = await _hierarchyDefinitionRepository.GetByIdAsync(command.HierarchyDefinitionId, cancellationToken);

            if (hierarchyDefinition is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyDefinition), command.HierarchyDefinitionId);
            }

            hierarchyDefinition.PrepareForDeletion();
            _hierarchyDefinitionRepository.Delete(hierarchyDefinition);
        }
    }
}
