using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RevokeInvitation
{
    public class RevokeInvitationCommandValidator : AbstractValidator<RevokeInvitationCommand>
    {
        public RevokeInvitationCommandValidator()
        {
            RuleFor(x => x.InvitationId).NotEmpty();
        }
    }
}
