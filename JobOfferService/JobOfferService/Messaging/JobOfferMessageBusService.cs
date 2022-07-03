using System.Text;
using Newtonsoft.Json;

using BusService;
using BusService.Routing;

using JobOfferService.Model;

namespace JobOfferService.JobOfferMessaging
{
    public class JobOfferMessageBusService : MessageBusHostedService
    {
        public JobOfferMessageBusService(IMessageBusService serviceBus) : base(serviceBus) { }

        protected override void ConfigureSubscribers()
        {
            // Add MessageSubscriber subscribers to the list of subscribers
            Subscribers.Add(new MessageBusSubscriber(
                SubjectBuilder.Build(Topics.JobOffer), 
                (sender, args) => {
                    var data = JsonConvert.DeserializeObject<JobOffer>(Encoding.UTF8.GetString(args.Message.Data));
                    if (data != null)
                    {
                        Console.WriteLine(data.Id);
                    }
                }
            ));
        }
    }
}
