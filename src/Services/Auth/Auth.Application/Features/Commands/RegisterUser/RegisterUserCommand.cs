using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string Email,
        string Password
    ) : ICommand<Guid>;
}
