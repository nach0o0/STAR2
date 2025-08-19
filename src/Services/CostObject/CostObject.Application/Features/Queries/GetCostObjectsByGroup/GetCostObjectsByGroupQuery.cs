using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByGroup
{
    public record GetCostObjectsByGroupQuery(Guid EmployeeGroupId) : IRequest<List<GetCostObjectsByGroupQueryResult>>;
}
