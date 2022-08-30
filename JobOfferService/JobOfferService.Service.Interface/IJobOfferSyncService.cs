using BusService.Contracts;
using JobOfferService.Model;

namespace JobOfferService.Service.Interface;

public interface IJobOfferSyncService : ISyncService<JobOffer, JobOfferContract>
{
}
