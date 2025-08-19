using MediatR;
using Planning.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningSummary
{
    public record GetPlanningSummaryQuery(
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate,
        SummaryGroupBy GroupBy
    ) : IRequest<List<GetPlanningSummaryQueryResult>>;
}
