using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectById
{
    public class GetCostObjectByIdQueryHandler : IRequestHandler<GetCostObjectByIdQuery, GetCostObjectByIdQueryResult?>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectByIdQueryHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<GetCostObjectByIdQueryResult?> Handle(GetCostObjectByIdQuery query, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectRepository.GetByIdAsync(query.CostObjectId, cancellationToken);

            if (costObject is null)
            {
                return null;
            }

            return new GetCostObjectByIdQueryResult(
                costObject.Id,
                costObject.Name,
                costObject.EmployeeGroupId,
                costObject.ParentCostObjectId,
                costObject.HierarchyLevelId,
                costObject.LabelId,
                costObject.ValidFrom,
                costObject.ValidTo,
                costObject.ApprovalStatus
            );
        }
    }
}
