using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyDefinition
{
    public class UpdateHierarchyDefinitionCommandHandler : IRequestHandler<UpdateHierarchyDefinitionCommand>
    {
        private readonly IHierarchyDefinitionRepository _hierarchyDefinitionRepository;

        public UpdateHierarchyDefinitionCommandHandler(IHierarchyDefinitionRepository hierarchyDefinitionRepository)
        {
            _hierarchyDefinitionRepository = hierarchyDefinitionRepository;
        }

        public async Task Handle(UpdateHierarchyDefinitionCommand command, CancellationToken cancellationToken)
        {
            var hierarchyDefinition = await _hierarchyDefinitionRepository.GetByIdAsync(command.HierarchyDefinitionId, cancellationToken);

            if (hierarchyDefinition is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyDefinition), command.HierarchyDefinitionId);
            }

            hierarchyDefinition.Update(command.Name, command.RequiredBookingLevelId);
        }
    }
}
