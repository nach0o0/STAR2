using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObject
{
    public record CreateCostObjectCommand(
        string Name,
        Guid EmployeeGroupId,
        Guid? ParentCostObjectId,
        Guid HierarchyLevelId,
        Guid? LabelId,
        DateTime ValidFrom
    ) : ICommand<Guid>;
}
