using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.CreateSession
{
    public record CreateSessionCommand(
        string BasicToken,
        string ClientInfo
    ) : IRequest<(string AccessToken, string RefreshToken)>;
}
