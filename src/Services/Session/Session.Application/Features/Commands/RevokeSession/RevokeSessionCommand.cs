using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.RevokeSession
{
    public record RevokeSessionCommand(string RefreshToken) : ICommand;
}
