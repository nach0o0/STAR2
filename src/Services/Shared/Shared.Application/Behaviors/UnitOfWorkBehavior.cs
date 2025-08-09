using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Application.Interfaces.Messaging;
using Shared.Application.Interfaces.Persistence;

namespace Shared.Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnitOfWorkBehavior<TRequest, TResponse>> _logger;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork, ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("[BEHAVIOR] UnitOfWork START for {RequestType}", typeof(TRequest).Name);
            // Führt zuerst den eigentlichen Handler aus.
            var response = await next();

            _logger.LogInformation("[BEHAVIOR] UnitOfWork COMMIT for {RequestType}", typeof(TRequest).Name);
            // Speichert danach alle Änderungen, die im Handler gemacht wurden.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("[BEHAVIOR] UnitOfWork END for {RequestType}", typeof(TRequest).Name);
            return response;
        }
    }
}
