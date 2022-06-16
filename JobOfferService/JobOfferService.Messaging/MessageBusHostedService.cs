using System;
using NATS.Client;
using Microsoft.Extensions.Hosting;

namespace JobOfferService.Messaging
{
    public abstract class MessageBusHostedService : IHostedService
    {
        private readonly IMessageBusService _serviceBus;
        private List<IAsyncSubscription> _eventSubscription = new List<IAsyncSubscription>();

        protected List<MessageBusSubscriber> Subscribers = new List<MessageBusSubscriber>();

        public MessageBusHostedService(IMessageBusService serviceBus)
        {
            _serviceBus = serviceBus;
            ConfigureSubscribers();
        }

        protected abstract void ConfigureSubscribers();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Subscribers.ForEach((subscriber) => _eventSubscription.Add(_serviceBus.SubscribeEvent(subscriber.Subject, subscriber.Handler)));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventSubscription.ForEach((subscription) => subscription.Unsubscribe());
            return Task.CompletedTask;
        }
    }
}
