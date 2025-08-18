using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateWorkModel
{
    public record UpdateWorkModelCommand(
        Guid WorkModelId,
        string? Name,
        decimal? MondayHours,
        decimal? TuesdayHours,
        decimal? WednesdayHours,
        decimal? ThursdayHours,
        decimal? FridayHours,
        decimal? SaturdayHours,
        decimal? SundayHours) : ICommand;
}
