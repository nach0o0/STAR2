using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyPendingInvitations
{
    public class GetMyPendingInvitationsQueryValidator : AbstractValidator<GetMyPendingInvitationsQuery>
    {
        public GetMyPendingInvitationsQueryValidator()
        {
            // Keine Regeln notwendig, da es keine Parameter gibt.
        }
    }
}
