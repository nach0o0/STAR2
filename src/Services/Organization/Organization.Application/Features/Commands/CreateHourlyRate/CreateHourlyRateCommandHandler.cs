using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateHourlyRate
{
    public class CreateHourlyRateCommandHandler : IRequestHandler<CreateHourlyRateCommand, Guid>
    {
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public CreateHourlyRateCommandHandler(IHourlyRateRepository hourlyRateRepository)
        {
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task<Guid> Handle(CreateHourlyRateCommand command, CancellationToken cancellationToken)
        {
            var hourlyRate = new HourlyRate(
                command.Name,
                command.Rate,
                command.ValidFrom,
                command.OrganizationId,
                command.ValidTo,
                command.Description);

            await _hourlyRateRepository.AddAsync(hourlyRate, cancellationToken);

            return hourlyRate.Id;
        }
    }
}
