using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByRole
{
    public class GetPermissionsByRoleQueryValidator : AbstractValidator<GetPermissionsByRoleQuery>
    {
        public GetPermissionsByRoleQueryValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
}
