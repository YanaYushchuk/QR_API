using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Observers;

public interface IQrProcessingObserver
{
    Task OnQrProcessingStarted(QrRequest request);
    Task OnQrProcessingCompleted(QrRequest request, QrResult result);
    Task OnQrProcessingFailed(QrRequest request, Exception exception);
}