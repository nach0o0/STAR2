using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : ICommand<(string AccessToken, string RefreshToken)>;
}
