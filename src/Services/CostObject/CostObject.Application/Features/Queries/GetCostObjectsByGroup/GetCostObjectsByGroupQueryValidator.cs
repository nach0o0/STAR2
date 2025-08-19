using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByGroup
{
    public class GetCostObjectsByGroupQueryValidator : AbstractValidator<GetCostObjectsByGroupQuery>
    {
        public GetCostObjectsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
