using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateLabel
{
    public record CreateLabelCommand(
        string Name,
        Guid EmployeeGroupId
    ) : ICommand<Guid>;
}
