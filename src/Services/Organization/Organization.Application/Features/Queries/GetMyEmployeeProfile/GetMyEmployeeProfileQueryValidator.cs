using FluentValidation;
using Organization.Application.Features.Queries.GetEmployeeInfoByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyEmployeeProfile
{
    public class GetMyEmployeeProfileQueryValidator : AbstractValidator<GetMyEmployeeProfileQuery>
    {
        public GetMyEmployeeProfileQueryValidator()
        {
            // Keine Validierungsregeln notwendig, da der Query keine Parameter hat.
        }
    }
}
