using Qr_API.Queries;

namespace Qr_API;

public class Mediator : IMediator
{
    private readonly IQrFacade _facade;
    public Mediator(IQrFacade facade) => _facade = facade;

    public async Task<TResult> Send<TResult>(IRequest<TResult> request)
    {
        switch (request)
        {
            case ProcessQrCommand cmd:
                var result = await _facade.ProcessQr(cmd.Request);
                return (TResult)(object)result;
            case GetQrByIdQuery query:
                var qr = await _facade.GetQrById(query.Id);
                return (TResult)(object)qr;
            default:
                throw new NotSupportedException();
        }
    }
}