using MassTransit;
using Permission.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Persistence;
using Shared.Messages.Events.AuthService;

namespace Permission.Application.Features.IntegrationEventConsumers
{
    public class UserAccountDeletedConsumer : IConsumer<UserAccountDeletedIntegrationEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserAccountDeletedConsumer(IUserPermissionAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserAccountDeletedIntegrationEvent> context)
        {
            var userId = context.Message.UserId;

            // Finde und lösche alle Zuweisungen für diesen Benutzer.
            var assignments = await _assignmentRepository.GetAssignmentsForUserAsync(userId, context.CancellationToken);
            if (!assignments.Any()) return;

            foreach (var assignment in assignments)
            {
                _assignmentRepository.Delete(assignment);
            }

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
