using MediatR;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Events.Employees
{
    public record EmployeeUnassignedFromOrganizationEvent(
        Employee Employee,
        Guid PreviousOrganizationId) : INotification;
}
