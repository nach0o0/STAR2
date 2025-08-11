using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAllPermissions
{
    public class GetAllPermissionsQueryValidator : AbstractValidator<GetAllPermissionsQuery>
    {
        public GetAllPermissionsQueryValidator()
        {
            // Keine Regeln notwendig, da es keine Parameter gibt.
        }
    }
}
