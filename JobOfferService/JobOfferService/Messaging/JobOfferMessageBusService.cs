using Polly;

using BusService;
using BusService.Routing;

using JobOfferService.Service.Interface;

namespace JobOfferService.JobOfferMessaging;
public class JobOfferMessageBusService : MessageBusHostedService
{
    public JobOfferMessageBusService(IMessageBusService serviceBus, IServiceScopeFactory serviceScopeFactory) : base(serviceBus, serviceScopeFactory)
    {
    }

    protected override void ConfigureSubscribers()
    {
        var policy = BuildPolicy();

        // Add MessageSubscriber subscribers to the list of subscribers
        Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Profile), typeof(IProfileSyncService)));
    }

    private Policy BuildPolicy()
    {
        return Policy
                .Handle<Exception>()
                .WaitAndRetry(5, _ => TimeSpan.FromSeconds(5), (exception, _, _, _) =>
                {
                    // TODO: here we should log unsuccessful try to handle event
                });
    }
}
