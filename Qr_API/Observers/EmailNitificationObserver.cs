using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Observers;

public class EmailNotificationObserver : IQrProcessingObserver
{
    private readonly ILogger<EmailNotificationObserver> _logger;

    public EmailNotificationObserver(ILogger<EmailNotificationObserver> logger)
    {
        _logger = logger;
    }

    public async Task OnQrProcessingStarted(QrRequest request)
    {
        _logger.LogInformation($"📧 Email: QR processing started for data: {request.Data}");
        // Simulate email sending
        await Task.Delay(10);
    }

    public async Task OnQrProcessingCompleted(QrRequest request, QrResult result)
    {
        _logger.LogInformation($"📧 Email: QR processing completed. Success: {result.Success}");
        // Send success email
        await Task.Delay(10);
    }

    public async Task OnQrProcessingFailed(QrRequest request, Exception exception)
    {
        _logger.LogError($"📧 Email: QR processing failed: {exception.Message}");
        // Send error email
        await Task.Delay(10);
    }
}