using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetLabelsByGroup
{
    public record GetLabelsByGroupQuery(Guid EmployeeGroupId) : IRequest<List<GetLabelsByGroupQueryResult>>;
}
