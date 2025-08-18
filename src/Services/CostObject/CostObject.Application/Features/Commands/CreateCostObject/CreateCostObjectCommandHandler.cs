using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObject
{
    public class CreateCostObjectCommandHandler : IRequestHandler<CreateCostObjectCommand, Guid>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public CreateCostObjectCommandHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<Guid> Handle(CreateCostObjectCommand command, CancellationToken cancellationToken)
        {
            var costObject = new Domain.Entities.CostObject(
                command.Name,
                command.EmployeeGroupId,
                command.ParentCostObjectId,
                command.HierarchyLevelId,
                command.LabelId,
                command.ValidFrom,
                isApprovedDirectly: true
            );

            await _costObjectRepository.AddAsync(costObject, cancellationToken);

            return costObject.Id;
        }
    }
}
