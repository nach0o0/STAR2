using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetSubOrganizations
{
    public class GetSubOrganizationsQueryValidator : AbstractValidator<GetSubOrganizationsQuery>
    {
        public GetSubOrganizationsQueryValidator()
        {
            RuleFor(x => x.ParentOrganizationId).NotEmpty();
        }
    }
}
