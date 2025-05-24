using Qr_API.DbContext;

namespace Qr_API.Queries;

public class GetQrByIdQuery : IRequest<QrCode>
{
    public int Id { get; }
    public GetQrByIdQuery(int id) => Id = id;
}