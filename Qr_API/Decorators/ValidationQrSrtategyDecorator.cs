using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Decorators;

public class ValidationQrStrategyDecorator : IQrStrategy
{
    private readonly IQrStrategy _strategy;
    private readonly ILogger<ValidationQrStrategyDecorator> _logger;

    public ValidationQrStrategyDecorator(IQrStrategy strategy, ILogger<ValidationQrStrategyDecorator> logger)
    {
        _strategy = strategy;
        _logger = logger;
    }

    public QrResult Process(QrRequest request)
    {
        // Pre-validation
        if (request == null)
        {
            _logger.LogWarning("🚫 Validation failed: Request is null");
            return new QrResult { Success = false };
        }

        if (string.IsNullOrEmpty(request.Data))
        {
            _logger.LogWarning("🚫 Validation failed: Data is null or empty");
            return new QrResult { Success = false };
        }

        if (request.Data.Length > 1000)
        {
            _logger.LogWarning($"🚫 Validation failed: Data too long ({request.Data.Length} chars)");
            return new QrResult { Success = false };
        }

        _logger.LogInformation("✅ Validation passed");
        return _strategy.Process(request);
    }
}