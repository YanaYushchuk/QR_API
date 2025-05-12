using Microsoft.EntityFrameworkCore;

namespace Qr_API.DbContext;

public class QrDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public QrDbContext(DbContextOptions<QrDbContext> options) : base(options) { }
    public DbSet<QrCode> QrCodes { get; set; }
}

public class QrCode
{
    public int Id { get; set; }
    public string Data { get; set; }
    public bool Processed { get; set; }
}