namespace Qr_API;

public class Mediator : IMediator
{
    private readonly IQrFacade _facade;
    public Mediator(IQrFacade facade) => _facade = facade;

    public async Task<TResult> Send<TResult>(IRequest<TResult> request)
    {
        if (request is ProcessQrCommand cmd)
        {
            var result = await _facade.ProcessQr(cmd.Request);
            return (TResult)(object)result;
        }
        throw new NotSupportedException();
    }
}