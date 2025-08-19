using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyDefinitionsByGroup
{
    public class GetHierarchyDefinitionsByGroupQueryValidator : AbstractValidator<GetHierarchyDefinitionsByGroupQuery>
    {
        public GetHierarchyDefinitionsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
