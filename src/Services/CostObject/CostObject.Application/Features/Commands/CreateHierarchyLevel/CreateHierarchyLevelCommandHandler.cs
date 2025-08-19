using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateHierarchyLevel
{
    public class CreateHierarchyLevelCommandHandler : IRequestHandler<CreateHierarchyLevelCommand, Guid>
    {
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;

        public CreateHierarchyLevelCommandHandler(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            _hierarchyLevelRepository = hierarchyLevelRepository;
        }

        public async Task<Guid> Handle(CreateHierarchyLevelCommand command, CancellationToken cancellationToken)
        {
            var hierarchyLevel = new HierarchyLevel(
                command.Name,
                command.Depth,
                command.HierarchyDefinitionId
            );

            await _hierarchyLevelRepository.AddAsync(hierarchyLevel, cancellationToken);

            return hierarchyLevel.Id;
        }
    }
}
