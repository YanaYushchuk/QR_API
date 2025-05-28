using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Observers;

public class AuditObserver : IQrProcessingObserver
{
    private readonly ILogger<AuditObserver> _logger;

    public AuditObserver(ILogger<AuditObserver> logger)
    {
        _logger = logger;
    }

    public async Task OnQrProcessingStarted(QrRequest request)
    {
        _logger.LogInformation($"🔍 Audit: QR processing audit started at {DateTime.UtcNow}");
        await Task.Delay(5);
    }

    public async Task OnQrProcessingCompleted(QrRequest request, QrResult result)
    {
        _logger.LogInformation($"🔍 Audit: QR processing audit completed at {DateTime.UtcNow}");
        await Task.Delay(5);
    }

    public async Task OnQrProcessingFailed(QrRequest request, Exception exception)
    {
        _logger.LogError($"🔍 Audit: QR processing audit - FAILURE at {DateTime.UtcNow}");
        await Task.Delay(5);
    }
}