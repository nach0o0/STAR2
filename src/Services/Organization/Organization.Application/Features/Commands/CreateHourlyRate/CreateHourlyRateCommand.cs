using Shared.Application.Interfaces.Messaging;

namespace Organization.Application.Features.Commands.CreateHourlyRate
{
    public record CreateHourlyRateCommand(
        string Name,
        decimal Rate,
        DateTime ValidFrom,
        Guid OrganizationId,
        DateTime? ValidTo,
        string? Description
    ) : ICommand<Guid>;
}
