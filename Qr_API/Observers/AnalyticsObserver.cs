using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Observers;

public class AnalyticsObserver : IQrProcessingObserver
{
    private readonly ILogger<AnalyticsObserver> _logger;

    public AnalyticsObserver(ILogger<AnalyticsObserver> logger)
    {
        _logger = logger;
    }

    public async Task OnQrProcessingStarted(QrRequest request)
    {
        _logger.LogInformation($"📊 Analytics: Tracking QR processing start");
        await Task.Delay(5);
    }

    public async Task OnQrProcessingCompleted(QrRequest request, QrResult result)
    {
        _logger.LogInformation($"📊 Analytics: QR processing metrics - Success: {result.Success}, Data length: {request.Data?.Length}");
        await Task.Delay(5);
    }

    public async Task OnQrProcessingFailed(QrRequest request, Exception exception)
    {
        _logger.LogWarning($"📊 Analytics: QR processing failure tracked");
        await Task.Delay(5);
    }
}