using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyDefinition
{
    public record UpdateHierarchyDefinitionCommand(
        Guid HierarchyDefinitionId,
        string? Name,
        Guid? RequiredBookingLevelId
    ) : ICommand;
}
