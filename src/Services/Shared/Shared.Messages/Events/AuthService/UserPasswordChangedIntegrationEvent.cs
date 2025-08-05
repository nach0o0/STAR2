using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AuthService
{
    public record UserPasswordChangedIntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}
