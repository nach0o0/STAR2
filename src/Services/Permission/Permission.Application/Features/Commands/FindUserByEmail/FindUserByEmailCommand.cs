using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.FindUserByEmail
{
    public record FindUserByEmailCommand(string Email, string Scope) : IRequest<FindUserByEmailResult>;
}
