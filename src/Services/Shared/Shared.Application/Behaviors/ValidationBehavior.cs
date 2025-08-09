using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("[BEHAVIOR] Validation START for {RequestType}", typeof(TRequest).Name);
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationFailures
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    g => g.Key,
                    g => g.Distinct().ToArray());

            if (errors.Any())
            {
                _logger.LogWarning("[BEHAVIOR] Validation FAILED for {RequestType}", typeof(TRequest).Name);
                throw new Exceptions.ValidationException(errors);
            }

            _logger.LogInformation("[BEHAVIOR] Validation END for {RequestType}", typeof(TRequest).Name);
            return await next();
        }
    }
}
