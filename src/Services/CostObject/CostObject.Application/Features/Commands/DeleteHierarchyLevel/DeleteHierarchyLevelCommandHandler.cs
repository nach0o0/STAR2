using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteHierarchyLevel
{
    public class DeleteHierarchyLevelCommandHandler : IRequestHandler<DeleteHierarchyLevelCommand>
    {
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;

        public DeleteHierarchyLevelCommandHandler(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            _hierarchyLevelRepository = hierarchyLevelRepository;
        }

        public async Task Handle(DeleteHierarchyLevelCommand command, CancellationToken cancellationToken)
        {
            var hierarchyLevel = await _hierarchyLevelRepository.GetByIdAsync(command.HierarchyLevelId, cancellationToken);

            if (hierarchyLevel is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyLevel), command.HierarchyLevelId);
            }

            hierarchyLevel.PrepareForDeletion();
            _hierarchyLevelRepository.Delete(hierarchyLevel);
        }
    }
}
