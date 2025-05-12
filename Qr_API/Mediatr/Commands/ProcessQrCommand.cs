using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API;

public class ProcessQrCommand : IRequest<QrResult>
{
    public QrRequest Request { get; }
    public ProcessQrCommand(QrRequest request) => Request = request;
}