using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsForUser
{
    public class GetPermissionsForUserQueryValidator : AbstractValidator<GetPermissionsForUserQuery>
    {
        public GetPermissionsForUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Scopes).NotEmpty();
        }
    }
}
