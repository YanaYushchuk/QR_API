using Qr_API.DbContext;
using Qr_API.Requests;
using Qr_API.Results;
using Qr_API.Observers;

namespace Qr_API;

public interface IQrFacade
{
    Task<QrResult> ProcessQr(QrRequest request);
    Task<QrCode> GetQrById(int id);
    void Subscribe(IQrProcessingObserver observer);
    void Unsubscribe(IQrProcessingObserver observer);
}

public class QrFacade : IQrFacade
{
    private readonly IQrStrategy _strategy;
    private readonly QrDbContext _db;
    private readonly List<IQrProcessingObserver> _observers = new();
    private readonly ILogger<QrFacade> _logger;

    public QrFacade(IQrStrategy strategy, QrDbContext db, ILogger<QrFacade> logger)
    {
        _strategy = strategy;
        _db = db;
        _logger = logger;
    }

    public void Subscribe(IQrProcessingObserver observer)
    {
        _observers.Add(observer);
        _logger.LogInformation($"Observer {observer.GetType().Name} subscribed");
    }

    public void Unsubscribe(IQrProcessingObserver observer)
    {
        _observers.Remove(observer);
        _logger.LogInformation($"Observer {observer.GetType().Name} unsubscribed");
    }

    public async Task<QrResult> ProcessQr(QrRequest request)
    {
        try
        {
            await NotifyObservers(o => o.OnQrProcessingStarted(request));
            var result = _strategy.Process(request);
            _db.QrCodes.Add(new QrCode { Data = request.Data, Processed = result.Success });
            await _db.SaveChangesAsync();
            await NotifyObservers(o => o.OnQrProcessingCompleted(request, result));
            return result;
        }
        catch (Exception ex)
        {
            await NotifyObservers(o => o.OnQrProcessingFailed(request, ex));
            throw;
        }
    }

    public async Task<QrCode> GetQrById(int id)
    {
        return await _db.QrCodes.FindAsync(id);
    }

    private async Task NotifyObservers(Func<IQrProcessingObserver, Task> action)
    {
        var tasks = _observers.Select(action);
        await Task.WhenAll(tasks);
    }
}