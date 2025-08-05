using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.PrivilegedResetPassword
{
    public record PrivilegedResetPasswordCommand(
        Guid UserId,
        string NewPassword
    ) : ICommand;
}
