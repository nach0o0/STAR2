using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByUserIds
{
    public record GetEmployeesByUserIdsQuery(List<Guid> UserIds)
    : IRequest<List<GetEmployeesByUserIdsQueryResult>>;
}
