using Microsoft.Extensions.Options;

using OpenTracing;
using Jaeger.Reporters;
using Jaeger;
using Jaeger.Senders.Thrift;
using Jaeger.Samplers;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;
using Prometheus;

using BusService;

using JobOfferService.Repository;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using JobOfferService.Middlwares;
using JobOfferService.JobOfferMessaging;

var builder = WebApplication.CreateBuilder(args);

// Default Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Nats
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.Configure<MessageBusSettings>(builder.Configuration.GetSection("Nats"));
builder.Services.AddHostedService<JobOfferMessageBusService>();

// Mongo
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// Repositories
builder.Services.AddScoped<IJobOfferRepository, JobOfferRepository>();

// Services
builder.Services.AddScoped<IJobOfferService, JobOfferService.Service.JobOfferService>();

// Sync services
builder.Services.AddScoped<IJobOfferSyncService, JobOfferService.Service.JobOfferSyncService>();
builder.Services.AddScoped<IProfileSyncService, JobOfferService.Service.ProfileSyncService>();

// Controllers
builder.Services.AddControllers();


// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JobOfferService", Version = "v1" });
});

// Tracing
builder.Services.AddOpenTracing();

builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender("host.docker.internal", 6831, 0))
                    .Build();
    var tracer = new Tracer.Builder(serviceName)
        // The constant sampler reports every span.
        .WithSampler(new ConstSampler(true))
        // LoggingReporter prints every reported span to the logging framework.
        .WithLoggerFactory(loggerFactory)
        .WithReporter(reporter)
        .Build();

    GlobalTracer.Register(tracer);

    return tracer;
});

builder.Services.Configure<HttpHandlerDiagnosticOptions>(options =>
        options.OperationNameResolver =
            request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Prometheus metrics
app.UseMetricServer();

app.Run();

namespace JobOfferService
{
    public partial class Program { }
}


