using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetTopLevelCostObjectsByGroup
{
    public class GetTopLevelCostObjectsByGroupQueryValidator : AbstractValidator<GetTopLevelCostObjectsByGroupQuery>
    {
        public GetTopLevelCostObjectsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
