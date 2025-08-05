using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string OldPassword,
        string NewPassword
    ) : ICommand;
}
