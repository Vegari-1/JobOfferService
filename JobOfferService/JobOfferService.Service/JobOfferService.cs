using System.Linq.Expressions;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using JobOfferService.Repository.Interface.Pagination;
using MongoDB.Bson;

namespace JobOfferService.Service;
public class JobOfferService : IJobOfferService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly IJobOfferSyncService _jobOffferSyncService;

    public JobOfferService(IProfileRepository profileRepository, IJobOfferRepository jobOfferRepository,
        IJobOfferSyncService jobOffferSyncService)
    {
        _profileRepository = profileRepository;
        _jobOfferRepository = jobOfferRepository;
        _jobOffferSyncService = jobOffferSyncService;
    }

    public async Task<JobOffer> Save(Guid userId, JobOffer jobOffer)
    {
        var profile = await _profileRepository.FindOneAsync(x => x.GlobalId == userId);
        if (profile == null)
            throw new InvalidOperationException("User is not existing!");

        jobOffer.GlobalId = Guid.NewGuid();
        jobOffer.CreatedBy = profile;

        await _jobOfferRepository.InsertOneAsync(jobOffer);
        return jobOffer;
    }

    public async Task<PagedList<JobOffer>> Filter(Guid userId, PaginationParams paginationParams, string? query)
    {
        var profile = await _profileRepository.FindOneAsync(x => x.GlobalId == userId);
        if (profile == null)
            throw new InvalidOperationException("User is not existing!");

        Expression<Func<JobOffer, bool>> filter = string.IsNullOrEmpty(query) ?
            GetJobOfferFilter(profile) :
            GetJobOfferFilter(profile, query);

        return await _jobOfferRepository.FilterByAsync(filter, paginationParams);
    }

    public Task<JobOffer> Get(Guid userId, string id)
    {
        var objectId = new ObjectId(id);
        return _jobOfferRepository.FindOneAsync(x => x.Id == objectId && x.CreatedBy.GlobalId == userId);
    }

    public async Task<JobOffer> Update(Guid userId, string id, JobOffer jobOffer)
    {
        var objectId = new ObjectId(id);
        var jobOfferFromDb = await _jobOfferRepository.FindOneAsync(x => x.Id == objectId && x.CreatedBy.GlobalId == userId);

        if (jobOfferFromDb == null)
            return default;

        jobOfferFromDb.PositionName = jobOffer.PositionName;
        jobOfferFromDb.Description = jobOffer.Description;
        jobOfferFromDb.Qualifications = jobOffer.Qualifications;
        jobOfferFromDb.CompanyLink = jobOffer.CompanyLink;

        await _jobOfferRepository.ReplaceOneAsync(jobOfferFromDb);
        return jobOfferFromDb;
    }

    public async Task Delete(Guid userId, string id)
    {
        var objectId = new ObjectId(id);
        var jobOffer = await _jobOfferRepository.FindOneAsync(x => x.Id == objectId && x.CreatedBy.GlobalId == userId);

        if (jobOffer == null)
            throw new InvalidOperationException("Job offer is not existing!");

        await _jobOfferRepository.DeleteByIdAsync(id);
    }

    private Expression<Func<JobOffer, bool>> GetJobOfferFilter(Profile profile, string query)
    {
        return (JobOffer jobOffer) =>
                !profile.Blocked.Contains(jobOffer.CreatedBy.GlobalId) &&
                !profile.BlockedBy.Contains(jobOffer.CreatedBy.GlobalId) &&
                (jobOffer.PositionName.ToLower().Contains(query.ToLower()) ||
                 jobOffer.Description.ToLower().Contains(query.ToLower()));
    }

    private Expression<Func<JobOffer, bool>> GetJobOfferFilter(Profile profile)
    {
        return (JobOffer jobOffer) =>
            !profile.Blocked.Contains(jobOffer.CreatedBy.GlobalId) &&
            !profile.BlockedBy.Contains(jobOffer.CreatedBy.GlobalId);
    }
}