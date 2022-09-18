using Polly;

using BusService;
using BusService.Routing;

using JobOfferService.Service.Interface;

namespace JobOfferService.JobOfferMessaging;
public class JobOfferMessageBusService : MessageBusHostedService
{
    private readonly ILogger<JobOfferMessageBusService> _logger;

    public JobOfferMessageBusService(ILogger<JobOfferMessageBusService> logger, IMessageBusService serviceBus,
        IServiceScopeFactory serviceScopeFactory) : base(serviceBus, serviceScopeFactory)
    {
        _logger = logger;
    }

    protected override void ConfigureSubscribers()
    {
        var policy = BuildPolicy();

        // Add MessageSubscriber subscribers to the list of subscribers
        Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Profile), typeof(IProfileSyncService)));
        Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Block), typeof(IConnectionSyncService)));
    }

    private Policy BuildPolicy()
    {
        return Policy
                .Handle<Exception>()
                .WaitAndRetry(5, _ => TimeSpan.FromSeconds(5), (exception, _, _, _) =>
                {
                    _logger.LogTrace($"Unsuccessfull try to sync event!", exception);
                });
    }
}
