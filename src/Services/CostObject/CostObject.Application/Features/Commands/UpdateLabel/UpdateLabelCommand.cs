using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateLabel
{
    public record UpdateLabelCommand(
        Guid LabelId,
        string Name
    ) : ICommand;
}
