using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyLevel
{
    public class UpdateHierarchyLevelCommandHandler : IRequestHandler<UpdateHierarchyLevelCommand>
    {
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;

        public UpdateHierarchyLevelCommandHandler(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            _hierarchyLevelRepository = hierarchyLevelRepository;
        }

        public async Task Handle(UpdateHierarchyLevelCommand command, CancellationToken cancellationToken)
        {
            var hierarchyLevel = await _hierarchyLevelRepository.GetByIdAsync(command.HierarchyLevelId, cancellationToken);

            if (hierarchyLevel is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyLevel), command.HierarchyLevelId);
            }

            hierarchyLevel.Update(command.Name, command.Depth);
        }
    }
}
