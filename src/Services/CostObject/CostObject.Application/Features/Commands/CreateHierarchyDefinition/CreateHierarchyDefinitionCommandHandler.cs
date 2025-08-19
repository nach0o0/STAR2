using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateHierarchyDefinition
{
    public class CreateHierarchyDefinitionCommandHandler : IRequestHandler<CreateHierarchyDefinitionCommand, Guid>
    {
        private readonly IHierarchyDefinitionRepository _hierarchyDefinitionRepository;

        public CreateHierarchyDefinitionCommandHandler(IHierarchyDefinitionRepository hierarchyDefinitionRepository)
        {
            _hierarchyDefinitionRepository = hierarchyDefinitionRepository;
        }

        public async Task<Guid> Handle(CreateHierarchyDefinitionCommand command, CancellationToken cancellationToken)
        {
            var hierarchyDefinition = new HierarchyDefinition(
                command.Name,
                command.EmployeeGroupId,
                command.RequiredBookingLevelId
            );

            await _hierarchyDefinitionRepository.AddAsync(hierarchyDefinition, cancellationToken);

            return hierarchyDefinition.Id;
        }
    }
}
