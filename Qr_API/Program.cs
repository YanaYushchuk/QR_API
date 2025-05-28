using Microsoft.EntityFrameworkCore;
using Qr_API;
using Qr_API.DbContext;
using Qr_API.Queries;
using Qr_API.Requests;
using Qr_API.Factories;
using Qr_API.Observers;
using Qr_API.Decorators;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QrDbContext>(options =>
    options.UseSqlite("Data Source=qr.db"));
builder.Services.AddScoped<IMediator, Mediator>();

builder.Services.AddScoped<IQrStrategyFactory, QrStrategyFactory>();

builder.Services.AddScoped<IQrProcessingObserver, EmailNotificationObserver>();
builder.Services.AddScoped<IQrProcessingObserver, AnalyticsObserver>();
builder.Services.AddScoped<IQrProcessingObserver, AuditObserver>();
/////////////////////////////////////////
/// 
/// 

builder.Services.AddScoped<IQrStrategy>(provider =>
{
    var factory = provider.GetRequiredService<IQrStrategyFactory>();
    var logger1 = provider.GetRequiredService<ILogger<LoggingQrStrategyDecorator>>();
    var logger2 = provider.GetRequiredService<ILogger<CachingQrStrategyDecorator>>();
    var logger3 = provider.GetRequiredService<ILogger<ValidationQrStrategyDecorator>>();

    var baseStrategy = factory.CreateStrategy(QrStrategyType.Advanced);

    var decoratedStrategy = new LoggingQrStrategyDecorator(
        new CachingQrStrategyDecorator(
            new ValidationQrStrategyDecorator(baseStrategy, logger3),
        logger2),
    logger1);

    return decoratedStrategy;
});

builder.Services.AddScoped<IQrFacade>((provider) =>
{
    var strategy = provider.GetRequiredService<IQrStrategy>();
    var db = provider.GetRequiredService<QrDbContext>();
    var logger = provider.GetRequiredService<ILogger<QrFacade>>();
    var observers = provider.GetServices<IQrProcessingObserver>();

    var facade = new QrFacade(strategy, db, logger);

    foreach (var observer in observers)
    {
        facade.Subscribe(observer);
    }

    return facade;
});

/// ////
/// ///////////////////////////////////////
//builder.Services.AddScoped<IQrFacade, QrFacade>();

//Select strategy based with if statement, for example on a configuration setting (Linux, Windows, etc.) or for example country
//builder.Services.AddScoped<IQrStrategy, DefaultQrStrategy>();


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

//New endpoint
app.MapPost("/api/qr/strategy/{strategyType}", async (QrRequest request, string strategyType, IQrStrategyFactory factory) =>
{
    if (!Enum.TryParse<QrStrategyType>(strategyType, true, out var type))
    {
        return Results.BadRequest($"Invalid strategy type: {strategyType}");
    }

    var strategy = factory.CreateStrategy(type);
    var result = strategy.Process(request);
    return Results.Ok(new { StrategyUsed = strategyType, Result = result });
});

app.Run();