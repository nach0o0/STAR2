using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject
{
    public class GetTimeEntriesByCostObjectQueryValidator : AbstractValidator<GetTimeEntriesByCostObjectQuery>
    {
        public GetTimeEntriesByCostObjectQueryValidator()
        {
            RuleFor(x => x.CostObjectId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
