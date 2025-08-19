using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByGroup
{
    public class GetTimeEntriesByGroupQueryValidator : AbstractValidator<GetTimeEntriesByGroupQuery>
    {
        public GetTimeEntriesByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
