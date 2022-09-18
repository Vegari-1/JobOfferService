using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using BusService;
using BusService.Contracts;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;
using SharpCompress.Common;

namespace JobOfferService.Service;

public class ConnectionSyncService : ConsumerBase<Connection, ConnectionContract>, IConnectionSyncService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IJobOfferRepository _jobOfferRepository;

    public ConnectionSyncService(IProfileRepository profileRepository, IJobOfferRepository jobOfferRepository,
        ILogger<ConnectionSyncService> logger) : base(logger)
    {
        _profileRepository = profileRepository;
        _jobOfferRepository = jobOfferRepository;
    }

    public override Task PublishAsync(Connection entity, string action)
    {
        throw new NotImplementedException();
    }

    public override async Task SynchronizeAsync(ConnectionContract entity, string action)
    {
        var blockerId = entity.Profile1;
        var blockedId = entity.Profile2;

        var profileFilter = Builders<Profile>.Filter.Eq(x => x.GlobalId, blockedId);
        var jobOfferFilter = Builders<JobOffer>.Filter.Eq(x => x.CreatedBy.GlobalId, blockedId);

        if (action == Events.Created)
        {
            // Update blocked by list for user who got blocked
            var updateBlockedProfile = Builders<Profile>.Update.AddToSet(x => x.BlockedBy, blockerId);
            await _profileRepository.UpdateManyAsync(BuildProfileFilter(blockedId), updateBlockedProfile);

            var updateBlockedJobOffer = Builders<JobOffer>.Update.AddToSet(x => x.CreatedBy.BlockedBy, blockerId);
            await _jobOfferRepository.UpdateManyAsync(BuildJobOfferFilter(blockedId), updateBlockedJobOffer);

            // Update blocked list for user who blocked
            var updateBlockerProfile = Builders<Profile>.Update.AddToSet(x => x.Blocked, blockedId);
            await _profileRepository.UpdateManyAsync(BuildProfileFilter(blockerId), updateBlockerProfile);

            var updateBlockerJobOffer = Builders<JobOffer>.Update.AddToSet(x => x.CreatedBy.Blocked, blockedId);
            await _jobOfferRepository.UpdateManyAsync(BuildJobOfferFilter(blockerId), updateBlockerJobOffer);

        }
        else if (action == Events.Deleted)
        {
            // Update blocked by list for user who got blocked
            var updateBlockedProfile = Builders<Profile>.Update.PullFilter(x => x.BlockedBy, y => y == blockerId);
            await _profileRepository.UpdateManyAsync(BuildProfileFilter(blockedId), updateBlockedProfile);

            var updateBlockedJobOffer = Builders<JobOffer>.Update.PullFilter(x => x.CreatedBy.BlockedBy, y => y == blockerId);
            await _jobOfferRepository.UpdateManyAsync(BuildJobOfferFilter(blockedId), updateBlockedJobOffer);

            // Update blocked list for user who blocked
            var updateBlockerProfile = Builders<Profile>.Update.PullFilter(x => x.Blocked, y => y == blockedId);
            await _profileRepository.UpdateManyAsync(BuildProfileFilter(blockerId), updateBlockerProfile);

            var updateBlockerJobOffer = Builders<JobOffer>.Update.PullFilter(x => x.CreatedBy.Blocked, y => y == blockedId);
            await _jobOfferRepository.UpdateManyAsync(BuildJobOfferFilter(blockerId), updateBlockerJobOffer);
        }
    }

    private FilterDefinition<Profile> BuildProfileFilter(Guid id) => Builders<Profile>.Filter.Eq(x => x.GlobalId, id);
    private FilterDefinition<JobOffer> BuildJobOfferFilter(Guid id) => Builders<JobOffer>.Filter.Eq(x => x.CreatedBy.GlobalId, id);
}

