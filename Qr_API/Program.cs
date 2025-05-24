using Microsoft.EntityFrameworkCore;
using Qr_API;
using Qr_API.DbContext;
using Qr_API.Queries;
using Qr_API.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QrDbContext>(options =>
    options.UseSqlite("Data Source=qr.db"));
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<IQrFacade, QrFacade>();

//Select strategy based with if statement, for example on a configuration setting (Linux, Windows, etc.) or for example country
builder.Services.AddScoped<IQrStrategy, DefaultQrStrategy>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/qr", async (QrRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(new ProcessQrCommand(request));
    return Results.Ok(result);
});

app.MapGet("/api/qr/{id:int}", async (int id, IMediator mediator) =>
{
    var qr = await mediator.Send(new GetQrByIdQuery(id));
    return qr is not null ? Results.Ok(qr) : Results.NotFound();
});

app.Run();