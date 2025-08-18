using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupById
{
    public record GetEmployeeGroupByIdQuery(Guid EmployeeGroupId)
        : IRequest<GetEmployeeGroupByIdQueryResult?>;
}
