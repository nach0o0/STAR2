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
    public class UserPasswordChangedEventHandlerTests
    {
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly UserPasswordChangedEventHandler _handler;

        public UserPasswordChangedEventHandlerTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _handler = new UserPasswordChangedEventHandler(_publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_Should_PublishUserPasswordChangedIntegrationEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var domainEvent = new UserPasswordChangedEvent(userId);

            // Act
            await _handler.Handle(domainEvent, CancellationToken.None);

            // Assert
            _publishEndpointMock.Verify(p => p.Publish(
                It.Is<UserPasswordChangedIntegrationEvent>(e => e.UserId == userId),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
