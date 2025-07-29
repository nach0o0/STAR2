using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateHourlyRate
{
    public record CreateHourlyRateCommand(
        string Name,
        decimal Rate,
        DateTime ValidFrom,
        Guid OrganizationId,
        DateTime? ValidTo,
        string? Description
    ) : IRequest<Guid>;
}
