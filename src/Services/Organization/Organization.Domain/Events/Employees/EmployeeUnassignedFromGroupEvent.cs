using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Events.Employees
{
    public record EmployeeUnassignedFromGroupEvent(Guid EmployeeId, Guid EmployeeGroupId) : INotification;
}
