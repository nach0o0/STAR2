using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Commands.AnonymizeTimeEntry
{
    public class AnonymizeTimeEntryCommandHandler : IRequestHandler<AnonymizeTimeEntryCommand, AnonymizeTimeEntryCommandResult>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public AnonymizeTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<AnonymizeTimeEntryCommandResult> Handle(AnonymizeTimeEntryCommand command, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(command.TimeEntryId, cancellationToken);

            if (timeEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), command.TimeEntryId);
            }

            timeEntry.Anonymize();

            return new AnonymizeTimeEntryCommandResult(timeEntry.Id, timeEntry.AccessKey!.Value);
        }
    }
}
