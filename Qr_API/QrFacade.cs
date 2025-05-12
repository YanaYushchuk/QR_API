using Qr_API.DbContext;
using Qr_API.Requests;
using Qr_API.Results;

namespace Qr_API;

public interface IQrFacade
{
    Task<QrResult> ProcessQr(QrRequest request);
}

public class QrFacade : IQrFacade
{
    private readonly IQrStrategy _strategy;
    private readonly QrDbContext _db;
    public QrFacade(IQrStrategy strategy, QrDbContext db)
    {
        _strategy = strategy;
        _db = db;
    }

    public async Task<QrResult> ProcessQr(QrRequest request)
    {
        var result = _strategy.Process(request);
        _db.QrCodes.Add(new QrCode { Data = request.Data, Processed = result.Success });
        await _db.SaveChangesAsync();
        return result;
    }
}