using BusService;
using BusService.Contracts;

namespace JobOfferService.Service.Interface
{
    public interface IEventSyncService : ISyncService<EventContract, EventContract>
    {
    }
}
