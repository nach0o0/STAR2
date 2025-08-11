using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByEmployeeGroup
{
    public record GetEmployeesByEmployeeGroupQuery(Guid EmployeeGroupId)
        : IRequest<List<GetEmployeesByEmployeeGroupQueryResult>>;
}
