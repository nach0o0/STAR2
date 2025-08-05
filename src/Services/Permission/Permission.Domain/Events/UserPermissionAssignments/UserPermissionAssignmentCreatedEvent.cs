using MediatR;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Events.UserPermissionAssignments
{
    public record UserPermissionAssignmentCreatedEvent(UserPermissionAssignment Assignment) : INotification;
}
