using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeactivateCostObject
{
    public class DeactivateCostObjectCommandHandler : IRequestHandler<DeactivateCostObjectCommand>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public DeactivateCostObjectCommandHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(DeactivateCostObjectCommand command, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectRepository.GetByIdAsync(command.CostObjectId, cancellationToken);

            if (costObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), command.CostObjectId);
            }

            costObject.Deactivate(command.ValidTo);

            // Speichern und Event-Publishing werden von der UnitOfWork-Pipeline übernommen.
        }
    }
}
