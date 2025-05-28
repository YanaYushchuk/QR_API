using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Factories;

public enum QrStrategyType
{
    Default,
    Advanced,
    Mobile,
    Enterprise
}

public interface IQrStrategyFactory
{
    IQrStrategy CreateStrategy(QrStrategyType type);
    IQrStrategy CreateStrategy(QrRequest request); // Smart factory based on request
}

public class QrStrategyFactory : IQrStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QrStrategyFactory> _logger;

    public QrStrategyFactory(IServiceProvider serviceProvider, ILogger<QrStrategyFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public IQrStrategy CreateStrategy(QrStrategyType type)
    {
        _logger.LogInformation($"Creating strategy of type: {type}");
        
        return type switch
        {
            QrStrategyType.Default => new DefaultQrStrategy(),
            QrStrategyType.Advanced => new AdvancedQrStrategy(),
            QrStrategyType.Mobile => new MobileQrStrategy(),
            QrStrategyType.Enterprise => new EnterpriseQrStrategy(_serviceProvider.GetService<ILogger<EnterpriseQrStrategy>>()),
            _ => throw new ArgumentException($"Unknown strategy type: {type}")
        };
    }

    public IQrStrategy CreateStrategy(QrRequest request)
    {
        // Smart factory - chooses strategy based on request content
        if (string.IsNullOrEmpty(request.Data))
            return CreateStrategy(QrStrategyType.Default);
        
        if (request.Data.StartsWith("http"))
            return CreateStrategy(QrStrategyType.Advanced);
        
        if (request.Data.Length > 100)
            return CreateStrategy(QrStrategyType.Enterprise);
        
        return CreateStrategy(QrStrategyType.Mobile);
    }
}