using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeletePublicHoliday
{
    public class DeletePublicHolidayCommandHandler : IRequestHandler<DeletePublicHolidayCommand>
    {
        private readonly IPublicHolidayRepository _publicHolidayRepository;

        public DeletePublicHolidayCommandHandler(IPublicHolidayRepository publicHolidayRepository)
        {
            _publicHolidayRepository = publicHolidayRepository;
        }

        public async Task Handle(DeletePublicHolidayCommand command, CancellationToken cancellationToken)
        {
            var publicHoliday = await _publicHolidayRepository.GetByIdAsync(command.PublicHolidayId, cancellationToken);
            if (publicHoliday is null)
            {
                throw new NotFoundException(nameof(PublicHoliday), command.PublicHolidayId);
            }

            publicHoliday.PrepareForDeletion();
            _publicHolidayRepository.Delete(publicHoliday);
        }
    }
}
