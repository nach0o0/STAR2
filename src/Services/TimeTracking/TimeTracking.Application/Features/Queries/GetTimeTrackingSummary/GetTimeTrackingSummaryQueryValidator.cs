using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeTrackingSummary
{
    public class GetTimeTrackingSummaryQueryValidator : AbstractValidator<GetTimeTrackingSummaryQuery>
    {
        public GetTimeTrackingSummaryQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
            RuleFor(x => x.GroupBy).IsInEnum();
        }
    }
}
