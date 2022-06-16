using JobOfferService.Model;

namespace JobOfferService.Service.Interface;
public interface IJobOfferService
{
    Task<JobOffer> Create(JobOffer JobOffer);
    Task<IList<JobOffer>> Filter();
    Task<JobOffer> PublishToQue(string Id);
}

