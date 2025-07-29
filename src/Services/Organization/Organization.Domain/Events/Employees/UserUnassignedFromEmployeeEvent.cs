using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Events.Employees
{
    public record UserUnassignedFromEmployeeEvent(Guid EmployeeId, Guid UserId) : INotification;
}
