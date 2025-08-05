using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Messaging
{
    public interface IBaseCommand { }

    // Für Commands, die eine Antwort zurückgeben
    public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand { }

    // Für Commands, die keine Antwort zurückgeben
    public interface ICommand : IRequest, IBaseCommand { }
}
