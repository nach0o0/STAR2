using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByEmployee
{
    public class GetPlanningEntriesByEmployeeQueryValidator : AbstractValidator<GetPlanningEntriesByEmployeeQuery>
    {
        public GetPlanningEntriesByEmployeeQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
