using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Events.Users
{
    public record UserAccountDeletedEvent(Guid UserId) : INotification;
}
