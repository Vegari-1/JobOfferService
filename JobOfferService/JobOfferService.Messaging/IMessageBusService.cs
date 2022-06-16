using NATS.Client;

namespace JobOfferService.Messaging
{
    public interface IMessageBusService
    {
        void PublishEvent(string subject, byte[] data);
        IAsyncSubscription SubscribeEvent(string subject, EventHandler<MsgHandlerEventArgs> handler);
        void UnsubscribeEvent(IAsyncSubscription subscription);
    }
}
