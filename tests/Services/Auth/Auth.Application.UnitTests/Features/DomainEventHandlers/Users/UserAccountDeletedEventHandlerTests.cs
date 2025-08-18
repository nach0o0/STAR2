using Auth.Application.Features.DomainEventHandlers.Users;
using Auth.Domain.Events.Users;
using MassTransit;
using Moq;
using Shared.Messages.Events.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.DomainEventHandlers.Users
{
    public class UserAccountDeletedEventHandlerTests
    {
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly UserAccountDeletedEventHandler _handler;

        public UserAccountDeletedEventHandlerTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _handler = new UserAccountDeletedEventHandler(_publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_Should_PublishUserAccountDeletedIntegrationEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var domainEvent = new UserAccountDeletedEvent(userId);

            // Act
            await _handler.Handle(domainEvent, CancellationToken.None);

            // Assert
            // Überprüft, ob die Publish-Methode genau einmal aufgerufen wurde...
            _publishEndpointMock.Verify(p => p.Publish(
                // ...mit einem Objekt vom Typ UserAccountDeletedIntegrationEvent...
                It.Is<UserAccountDeletedIntegrationEvent>(e => e.UserId == userId),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
