using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteHourlyRate
{
    public record DeleteHourlyRateCommand(Guid HourlyRateId) : IRequest;
}
