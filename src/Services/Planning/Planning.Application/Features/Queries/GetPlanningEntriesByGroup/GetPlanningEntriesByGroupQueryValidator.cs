using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByGroup
{
    public class GetPlanningEntriesByGroupQueryValidator : AbstractValidator<GetPlanningEntriesByGroupQuery>
    {
        public GetPlanningEntriesByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
