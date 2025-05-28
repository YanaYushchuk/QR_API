using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Decorators;

public class LoggingQrStrategyDecorator : IQrStrategy
{
    private readonly IQrStrategy _strategy;
    private readonly ILogger<LoggingQrStrategyDecorator> _logger;

    public LoggingQrStrategyDecorator(IQrStrategy strategy, ILogger<LoggingQrStrategyDecorator> logger)
    {
        _strategy = strategy;
        _logger = logger;
    }

    public QrResult Process(QrRequest request)
    {
        _logger.LogInformation($"🔄 Processing QR request - Data: {request.Data}");
        var startTime = DateTime.UtcNow;

        try
        {
            var result = _strategy.Process(request);
            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation($"✅ QR processing completed - Success: {result.Success}, Duration: {duration.TotalMilliseconds}ms");
            return result;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError($"❌ QR processing failed - Duration: {duration.TotalMilliseconds}ms, Error: {ex.Message}");
            throw;
        }
    }
}