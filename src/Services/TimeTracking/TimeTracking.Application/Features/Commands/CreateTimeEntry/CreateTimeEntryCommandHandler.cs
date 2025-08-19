using MediatR;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Application.Features.Commands.CreateTimeEntry
{
    public class CreateTimeEntryCommandHandler : IRequestHandler<CreateTimeEntryCommand, CreateTimeEntryCommandResult>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUserContext _userContext;
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public CreateTimeEntryCommandHandler(
            ITimeEntryRepository timeEntryRepository,
            IUserContext userContext,
            ICostObjectServiceClient costObjectServiceClient)
        {
            _timeEntryRepository = timeEntryRepository;
            _userContext = userContext;
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task<CreateTimeEntryCommandResult> Handle(CreateTimeEntryCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            if (!command.CostObjectId.HasValue)
            {
                throw new InvalidOperationException("A cost object must be selected.");
            }

            var costObject = await _costObjectServiceClient.GetByIdAsync(command.CostObjectId.Value, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException("CostObject", command.CostObjectId.Value);
            }

            if (!currentUser.EmployeeGroupIds.Contains(costObject.EmployeeGroupId))
            {
                throw new ForbiddenAccessException("You are not a member of the cost object's employee group.");
            }

            var timeEntry = new TimeEntry(
                currentUser.EmployeeId,
                command.CostObjectId,
                costObject.EmployeeGroupId,
                command.EntryDate,
                command.Hours,
                command.HourlyRate,
                command.Description,
                command.CreateAnonymously
            );

            await _timeEntryRepository.AddAsync(timeEntry, cancellationToken);

            // Gib die ID und den eventuell generierten AccessKey zurück.
            return new CreateTimeEntryCommandResult(timeEntry.Id, timeEntry.AccessKey);
        }
    }
}
