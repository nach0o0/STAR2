using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationById
{
    public record GetOrganizationByIdQuery(Guid OrganizationId)
        : IRequest<GetOrganizationByIdQueryResult?>;
}
