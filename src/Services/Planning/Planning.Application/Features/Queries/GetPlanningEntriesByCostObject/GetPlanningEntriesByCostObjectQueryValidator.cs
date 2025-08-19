using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByCostObject
{
    public class GetPlanningEntriesByCostObjectQueryValidator : AbstractValidator<GetPlanningEntriesByCostObjectQuery>
    {
        public GetPlanningEntriesByCostObjectQueryValidator()
        {
            RuleFor(x => x.CostObjectId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
