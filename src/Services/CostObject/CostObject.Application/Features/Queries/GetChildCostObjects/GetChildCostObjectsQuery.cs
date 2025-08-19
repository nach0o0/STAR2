using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetChildCostObjects
{
    public record GetChildCostObjectsQuery(Guid ParentCostObjectId) : IRequest<List<GetChildCostObjectsQueryResult>>;
}
