using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMySentInvitations
{
    public class GetMySentInvitationsQueryValidator : AbstractValidator<GetMySentInvitationsQuery>
    {
        public GetMySentInvitationsQueryValidator()
        {
            // Keine Regeln notwendig
        }
    }
}
