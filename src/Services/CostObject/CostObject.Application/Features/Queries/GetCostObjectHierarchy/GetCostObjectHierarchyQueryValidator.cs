using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectHierarchy
{
    public class GetCostObjectHierarchyQueryValidator : AbstractValidator<GetCostObjectHierarchyQuery>
    {
        public GetCostObjectHierarchyQueryValidator()
        {
            RuleFor(x => x.RootCostObjectId).NotEmpty();
        }
    }
}
