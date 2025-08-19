using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetActiveCostObjectsByGroup
{
    public class GetActiveCostObjectsByGroupQueryValidator : AbstractValidator<GetActiveCostObjectsByGroupQuery>
    {
        public GetActiveCostObjectsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
