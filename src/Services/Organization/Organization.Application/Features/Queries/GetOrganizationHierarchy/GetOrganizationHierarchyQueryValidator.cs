using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationHierarchy
{
    public class GetOrganizationHierarchyQueryValidator : AbstractValidator<GetOrganizationHierarchyQuery>
    {
        public GetOrganizationHierarchyQueryValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
        }
    }
}
