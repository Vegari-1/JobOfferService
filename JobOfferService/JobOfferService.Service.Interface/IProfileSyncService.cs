using BusService;
using BusService.Contracts;

using JobOfferService.Model;

namespace JobOfferService.Service.Interface;

public interface IProfileSyncService : ISyncService<Profile, ProfileContract>
{
}
