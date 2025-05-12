using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API;

public interface IQrStrategy
{
    QrResult Process(QrRequest request);
}

public class DefaultQrStrategy : IQrStrategy
{
    public QrResult Process(QrRequest request)
    {
        // Simple logic for demo
        return new QrResult { Success = !string.IsNullOrEmpty(request.Data) };
    }
}