using NATS.Client;

namespace JobOfferService.Messaging
{
    public class MessageBusSubscriber
    {
        public string Subject;   
        public EventHandler<MsgHandlerEventArgs> Handler;

        public MessageBusSubscriber(string subject, EventHandler<MsgHandlerEventArgs> handler)
        {
            Subject = subject;
            Handler = handler;
        }

    }
}
