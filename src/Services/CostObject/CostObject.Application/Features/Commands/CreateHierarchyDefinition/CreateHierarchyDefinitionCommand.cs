using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateHierarchyDefinition
{
    public record CreateHierarchyDefinitionCommand(
        string Name,
        Guid EmployeeGroupId,
        Guid? RequiredBookingLevelId
    ) : ICommand<Guid>;
}
