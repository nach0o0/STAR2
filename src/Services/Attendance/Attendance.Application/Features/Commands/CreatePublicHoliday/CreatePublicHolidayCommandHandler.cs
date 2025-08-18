using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreatePublicHoliday
{
    public class CreatePublicHolidayCommandHandler : IRequestHandler<CreatePublicHolidayCommand, Guid>
    {
        private readonly IPublicHolidayRepository _publicHolidayRepository;

        public CreatePublicHolidayCommandHandler(IPublicHolidayRepository publicHolidayRepository)
        {
            _publicHolidayRepository = publicHolidayRepository;
        }

        public async Task<Guid> Handle(CreatePublicHolidayCommand command, CancellationToken cancellationToken)
        {
            var publicHoliday = new PublicHoliday(
                command.Date,
                command.Name,
                command.EmployeeGroupId
            );

            await _publicHolidayRepository.AddAsync(publicHoliday, cancellationToken);

            return publicHoliday.Id;
        }
    }
}
