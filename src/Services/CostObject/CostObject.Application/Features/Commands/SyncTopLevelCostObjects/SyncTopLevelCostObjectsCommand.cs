using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.SyncTopLevelCostObjects
{
    public record SyncTopLevelCostObjectsCommand(
        Guid EmployeeGroupId,
        List<string> Names,
        DateTime ValidFrom,
        Guid TopHierarchyLevelId
    ) : ICommand;
}
