using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;

namespace JobOfferService.Service;
public class JobOfferService : IJobOfferService
{

    private readonly IJobOfferRepository _jobOfferRepository;

    public JobOfferService(IJobOfferRepository jobOfferRepository)
    {
        _jobOfferRepository = jobOfferRepository;
    }

    public Task<JobOffer> Create(JobOffer jobOffer)
    {
        throw new NotImplementedException();
    }

    public Task<IList<JobOffer>> Filter()
    {
        return Task.Run<IList<JobOffer>>(() => _jobOfferRepository.AsQueryable().ToList());
    }
}

