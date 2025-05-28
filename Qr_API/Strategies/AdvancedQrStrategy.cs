using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API.Factories;

public class AdvancedQrStrategy : IQrStrategy
{
    public QrResult Process(QrRequest request)
    {
        // Advanced validation for URLs
        if (Uri.TryCreate(request.Data, UriKind.Absolute, out var uri))
        {
            return new QrResult { Success = uri.Scheme == "http" || uri.Scheme == "https" };
        }
        return new QrResult { Success = false };
    }
}