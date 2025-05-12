namespace Qr_API;

public interface IMediator
{
    Task<TResult> Send<TResult>(IRequest<TResult> request);
}