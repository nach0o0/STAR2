using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetMyCostObjectRequests
{
    public record GetMyCostObjectRequestsQuery(Guid EmployeeGroupId) : IRequest<List<GetMyCostObjectRequestsQueryResult>>;
}
