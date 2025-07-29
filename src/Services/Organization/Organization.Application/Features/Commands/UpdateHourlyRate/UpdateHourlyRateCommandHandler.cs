using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateHourlyRate
{
    public class UpdateHourlyRateCommandHandler : IRequestHandler<UpdateHourlyRateCommand>
    {
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public UpdateHourlyRateCommandHandler(IHourlyRateRepository hourlyRateRepository)
        {
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task Handle(UpdateHourlyRateCommand command, CancellationToken cancellationToken)
        {
            var hourlyRate = await _hourlyRateRepository.GetByIdAsync(command.HourlyRateId, cancellationToken);
            if (hourlyRate is null)
            {
                throw new NotFoundException(nameof(HourlyRate), command.HourlyRateId);
            }

            hourlyRate.Update(
                command.Name,
                command.Rate,
                command.ValidFrom,
                command.ValidTo,
                command.Description);
        }
    }
}
