using CostObject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectRequestsByGroup
{
    public record GetCostObjectRequestsByGroupQuery(
        Guid EmployeeGroupId,
        ApprovalStatus? Status // Optionaler Filter
    ) : IRequest<List<GetCostObjectRequestsByGroupQueryResult>>;
}
