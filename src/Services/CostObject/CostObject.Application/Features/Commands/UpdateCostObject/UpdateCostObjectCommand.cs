using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateCostObject
{
    public record UpdateCostObjectCommand(
        Guid CostObjectId,
        string? Name,
        Guid? ParentCostObjectId,
        Guid? HierarchyLevelId,
        Guid? LabelId
    ) : ICommand;
}
