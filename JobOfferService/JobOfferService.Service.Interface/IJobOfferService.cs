using JobOfferService.Model;

namespace JobOfferService.Service.Interface;
public interface IJobOfferService
{
    Task<JobOffer> Create(JobOffer jobOffer);

    Task<IList<JobOffer>> Filter();
}

