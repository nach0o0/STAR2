using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange
{
    public class GetMyTimeEntriesByDateRangeQueryValidator : AbstractValidator<GetMyTimeEntriesByDateRangeQuery>
    {
        public GetMyTimeEntriesByDateRangeQueryValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be after or the same as start date.");
        }
    }
}
