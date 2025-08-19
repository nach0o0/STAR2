using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyLevel
{
    public record UpdateHierarchyLevelCommand(
        Guid HierarchyLevelId,
        string? Name,
        int? Depth
    ) : ICommand;
}
