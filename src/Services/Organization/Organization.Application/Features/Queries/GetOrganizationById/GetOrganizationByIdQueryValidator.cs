using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryValidator : AbstractValidator<GetOrganizationByIdQuery>
    {
        public GetOrganizationByIdQueryValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
        }
    }
}
