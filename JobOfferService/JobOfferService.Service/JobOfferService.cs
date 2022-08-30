using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using PostService.Repository.Interface.Pagination;

namespace JobOfferService.Service;
public class JobOfferService : IJobOfferService
{
    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly IJobOfferSyncService _jobOffferSyncService;

    public JobOfferService(IJobOfferRepository jobOfferRepository, IJobOfferSyncService jobOffferSyncService)
    {
        _jobOfferRepository = jobOfferRepository;
        _jobOffferSyncService = jobOffferSyncService;
    }

    public async Task<JobOffer> Save(JobOffer jobOffer)
    {
        jobOffer.GlobalId = Guid.NewGuid();
        await _jobOfferRepository.InsertOneAsync(jobOffer);
        return jobOffer;
    }

    public Task<PagedList<JobOffer>> Get(PaginationParams paginationParams)
    {
        return _jobOfferRepository.FilterByAsync(_ => true, paginationParams);
    }
}

