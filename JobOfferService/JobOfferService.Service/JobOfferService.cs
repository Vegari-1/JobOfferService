using System.Linq.Expressions;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using JobOfferService.Repository.Interface.Pagination;

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

    public Task<PagedList<JobOffer>> Filter(PaginationParams paginationParams, string? query)
    {
        Expression<Func<JobOffer, bool>> filter = string.IsNullOrEmpty(query) ?
            (JobOffer x) => true :
            (JobOffer x) =>
                x.PositionName.ToLower().Contains(query.ToLower()) ||
                x.Description.ToLower().Contains(query.ToLower());

        return _jobOfferRepository.FilterByAsync(filter, paginationParams);
    }

    public Task<JobOffer> Get(string id)
    {
        return _jobOfferRepository.FindByIdAsync(id);
    }

    public async Task<JobOffer> Update(string id, JobOffer jobOffer)
    {
        var jobOfferFromDb = await _jobOfferRepository.FindByIdAsync(id);

        if (jobOfferFromDb == null)
            return default;

        jobOfferFromDb.PositionName = jobOffer.PositionName;
        jobOfferFromDb.Description = jobOffer.Description;
        jobOfferFromDb.Qualifications = jobOffer.Qualifications;
        jobOfferFromDb.CompanyLink = jobOffer.CompanyLink;

        await _jobOfferRepository.ReplaceOneAsync(jobOfferFromDb);
        return jobOfferFromDb;
    }

    public async Task Delete(string id)
    {
        await _jobOfferRepository.DeleteByIdAsync(id);
    }
}