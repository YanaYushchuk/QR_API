using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Factories;

public class MobileQrStrategy : IQrStrategy
{
    public QrResult Process(QrRequest request)
    {
        // Mobile-optimized processing
        var success = !string.IsNullOrEmpty(request.Data) && request.Data.Length <= 50;
        return new QrResult { Success = success };
    }
}