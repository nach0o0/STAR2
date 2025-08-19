using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetChildCostObjects
{
    public class GetChildCostObjectsQueryHandler : IRequestHandler<GetChildCostObjectsQuery, List<GetChildCostObjectsQueryResult>>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetChildCostObjectsQueryHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<List<GetChildCostObjectsQueryResult>> Handle(GetChildCostObjectsQuery query, CancellationToken cancellationToken)
        {
            var children = await _costObjectRepository.GetChildrenAsync(query.ParentCostObjectId, cancellationToken);

            var result = new List<GetChildCostObjectsQueryResult>();
            foreach (var child in children)
            {
                // Prüfe für jedes Kind, ob es selbst Kinder hat.
                var hasChildren = await _costObjectRepository.HasChildrenAsync(child.Id, cancellationToken);
                result.Add(new GetChildCostObjectsQueryResult(
                    child.Id,
                    child.Name,
                    child.ApprovalStatus,
                    hasChildren
                ));
            }

            return result;
        }
    }
}
