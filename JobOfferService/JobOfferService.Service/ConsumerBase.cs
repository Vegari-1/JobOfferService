using System.Text;

using Newtonsoft.Json;
using Polly;

using BusService;
using BusService.Routing;

using JobOfferService.Service.Interface;

namespace JobOfferService.Service;

public abstract class ConsumerBase<TEntity, TContract> : ISyncService<TEntity, TContract>, IConsumer
{
    public Task Consume(string sender, byte[] data, Policy policy)
    {
        return Task.Run(() =>
            {
                try
                {
                    policy.Execute(async () =>
                    {
                        var entity = JsonConvert.DeserializeObject<TContract>(Encoding.UTF8.GetString(data));

                        if (entity == null)
                            throw new InvalidOperationException();

                        await SynchronizeAsync(entity, SubjectBuilder.GetEventName(sender));
                    });
                }
                catch (Exception e)
                {
                    // TODO: log unsuccessfull try
                }
            });
    }

    public abstract Task PublishAsync(TEntity entity, string action);

    public abstract Task SynchronizeAsync(TContract entity, string action);
}
