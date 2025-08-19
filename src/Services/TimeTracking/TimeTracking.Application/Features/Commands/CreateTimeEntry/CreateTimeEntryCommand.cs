using Shared.Application.Interfaces.Messaging;

namespace TimeTracking.Application.Features.Commands.CreateTimeEntry
{
    public record CreateTimeEntryCommand(
        Guid? EmployeeId,
        Guid? CostObjectId,
        DateTime EntryDate,
        decimal Hours,
        decimal HourlyRate,
        string? Description,
        bool CreateAnonymously
    ) : ICommand<CreateTimeEntryCommandResult>;
}
