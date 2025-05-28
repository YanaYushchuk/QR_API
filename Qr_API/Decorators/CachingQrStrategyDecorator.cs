using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Decorators;

public class CachingQrStrategyDecorator : IQrStrategy
{
    private readonly IQrStrategy _strategy;
    private readonly ILogger<CachingQrStrategyDecorator> _logger;
    private static readonly Dictionary<string, QrResult> _cache = new();

    public CachingQrStrategyDecorator(IQrStrategy strategy, ILogger<CachingQrStrategyDecorator> logger)
    {
        _strategy = strategy;
        _logger = logger;
    }

    public QrResult Process(QrRequest request)
    {
        var cacheKey = request.Data ?? string.Empty;

        if (_cache.TryGetValue(cacheKey, out var cachedResult))
        {
            _logger.LogInformation($"🚀 Cache HIT for key: {cacheKey}");
            return cachedResult;
        }

        _logger.LogInformation($"💾 Cache MISS for key: {cacheKey}");
        var result = _strategy.Process(request);

        _cache[cacheKey] = result;
        _logger.LogInformation($"💾 Result cached for key: {cacheKey}");

        return result;
    }
}