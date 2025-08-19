using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntryById
{
    public record GetTimeEntryByIdQuery(Guid TimeEntryId) : IRequest<GetTimeEntryByIdQueryResult?>;
}
