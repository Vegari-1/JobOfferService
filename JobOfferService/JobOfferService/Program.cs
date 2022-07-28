using Microsoft.Extensions.Options;

using BusService;

using JobOfferService.Repository;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using JobOfferService.Middlwares;
using JobOfferService.JobOfferMessaging;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();

public partial class Program { }

