using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.DeleteMyAccount
{
    public record DeleteMyAccountCommand(string Password) : ICommand;
}
