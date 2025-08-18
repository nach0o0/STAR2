using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupsByOrganization
{
    public class GetEmployeeGroupsByOrganizationQueryValidator : AbstractValidator<GetEmployeeGroupsByOrganizationQuery>
    {
        public GetEmployeeGroupsByOrganizationQueryValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
        }
    }
}
