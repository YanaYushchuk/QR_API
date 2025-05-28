using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Factories;

public class EnterpriseQrStrategy : IQrStrategy
{
    private readonly ILogger<EnterpriseQrStrategy> _logger;

    public EnterpriseQrStrategy(ILogger<EnterpriseQrStrategy> logger)
    {
        _logger = logger;
    }

    public QrResult Process(QrRequest request)
    {
        _logger?.LogInformation($"Enterprise processing for data length: {request.Data?.Length}");

        // Complex enterprise logic
        var success = !string.IsNullOrEmpty(request.Data) &&
                     request.Data.Length > 10 &&
                     !request.Data.Contains("test");

        return new QrResult { Success = success };
    }
}
