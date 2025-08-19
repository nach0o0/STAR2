using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteHierarchyDefinition
{
    public record DeleteHierarchyDefinitionCommand(Guid HierarchyDefinitionId) : ICommand;
}
