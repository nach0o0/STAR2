using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetTopLevelOrganizations
{
    public class GetTopLevelOrganizationsQueryValidator : AbstractValidator<GetTopLevelOrganizationsQuery>
    {
        public GetTopLevelOrganizationsQueryValidator()
        {
            // Keine Regeln notwendig
        }
    }
}
