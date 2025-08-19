using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyLevelsByDefinition
{
    public class GetHierarchyLevelsByDefinitionQueryValidator : AbstractValidator<GetHierarchyLevelsByDefinitionQuery>
    {
        public GetHierarchyLevelsByDefinitionQueryValidator()
        {
            RuleFor(x => x.HierarchyDefinitionId).NotEmpty();
        }
    }
}
