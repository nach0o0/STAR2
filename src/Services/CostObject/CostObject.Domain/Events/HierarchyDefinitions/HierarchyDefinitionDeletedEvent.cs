using CostObject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Events.HierarchyDefinitions
{
    public record HierarchyDefinitionDeletedEvent(HierarchyDefinition HierarchyDefinition) : INotification;
}
