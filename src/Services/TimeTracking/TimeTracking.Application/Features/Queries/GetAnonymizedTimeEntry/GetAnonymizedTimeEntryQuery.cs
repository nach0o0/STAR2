using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetAnonymizedTimeEntry
{
    public record GetAnonymizedTimeEntryQuery(Guid AccessKey) : IRequest<GetAnonymizedTimeEntryQueryResult?>;
}
