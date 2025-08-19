using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByIds
{
    public record GetEmployeesByIdsQuery(List<Guid> EmployeeIds) : IRequest<List<GetEmployeesByIdsQueryResult>>;
}
