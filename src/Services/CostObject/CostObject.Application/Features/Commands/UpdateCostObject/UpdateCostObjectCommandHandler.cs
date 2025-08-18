using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateCostObject
{
    public class UpdateCostObjectCommandHandler : IRequestHandler<UpdateCostObjectCommand>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public UpdateCostObjectCommandHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(UpdateCostObjectCommand command, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectRepository.GetByIdAsync(command.CostObjectId, cancellationToken);

            if (costObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), command.CostObjectId);
            }

            // Ruft die Update-Methode auf der Entität auf.
            costObject.Update(
                command.Name,
                command.ParentCostObjectId,
                command.HierarchyLevelId,
                command.LabelId
            );
        }
    }
}
