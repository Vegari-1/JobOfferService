using System.Text;

using NATS.Client;
using Newtonsoft.Json;

using BusService;
using BusService.Contracts;
using BusService.Routing;

namespace JobOfferService.IntegrationTests;

public static class BusExtensions
{
    public static void PublishProfileUpdate(this IntegrationWebApplicationFactory<Program> factory, ProfileContract profile) 
    {
        var connection = new ConnectionFactory().CreateConnection($"nats://{factory.natsContainer.Hostname}:4222");
        var serialized = JsonConvert.SerializeObject(profile);
        var bData = Encoding.UTF8.GetBytes(serialized); 
        connection.Publish(SubjectBuilder.Build(Topics.Profile, Events.Updated), bData);
    }
}
