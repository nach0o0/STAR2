using MediatR;
using Shared.Application.Interfaces.Messaging;
using Shared.Application.Interfaces.Persistence;

namespace Shared.Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Führt zuerst den eigentlichen Handler aus.
            var response = await next();

            // Speichert danach alle Änderungen, die im Handler gemacht wurden.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
