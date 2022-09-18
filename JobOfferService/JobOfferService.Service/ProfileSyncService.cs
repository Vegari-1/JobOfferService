using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using BusService;
using BusService.Contracts;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;

namespace JobOfferService.Service;
public class ProfileSyncService : ConsumerBase<Profile, ProfileContract>, IProfileSyncService
{
    private readonly IMessageBusService _messageBusService;
    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly IProfileRepository _profileRepository;

    public ProfileSyncService(IMessageBusService messageBusService, IJobOfferRepository jobOfferRepository,
        IProfileRepository profileRepository, ILogger<ProfileSyncService> logger) : base(logger)
    {
        _messageBusService = messageBusService;
        _jobOfferRepository = jobOfferRepository;
        _profileRepository = profileRepository;
    }

    public override Task PublishAsync(Profile entity, string action)
    {
        // Implement when it is needed
        throw new NotImplementedException();
    }

    public override async Task SynchronizeAsync(ProfileContract entity, string action)
    {
        var filterJobOffers = Builders<JobOffer>.Filter.Eq(x => x.CreatedBy.GlobalId, entity.Id);
        var filterProfiles = Builders<Profile>.Filter.Eq(x => x.GlobalId, entity.Id);
        if (action == Events.Created)
        {
            var profile = new Profile
            {
                GlobalId = entity.Id,
                Avatar = entity.Avatar,
                Name = entity.Name,
                Surname = entity.Surname
            };
            await _profileRepository.InsertOneAsync(profile);
        }
        else if (action == Events.Updated)
        {
            var updateProfiles = Builders<Profile>.Update
                .Set(x => x.Name, entity.Name)
                .Set(x => x.Surname, entity.Surname)
                .Set(x => x.Avatar, entity.Avatar);
            await _profileRepository.UpdateManyAsync(filterProfiles, updateProfiles);

            var updateJobOffer = Builders<JobOffer>.Update
                .Set(x => x.CreatedBy.Name, entity.Name)
                .Set(x => x.CreatedBy.Surname, entity.Surname)
                .Set(x => x.CreatedBy.Avatar, entity.Avatar);
            await _jobOfferRepository.UpdateManyAsync(filterJobOffers, updateJobOffer);
        }
        else if (action == Events.Deleted)
        {
            var updateProfiles = Builders<Profile>.Update
                .Set(x => x.Name, "Removed user")
                .Set(x => x.Surname, "")
                .Set(x => x.Avatar, "");
            await _profileRepository.UpdateManyAsync(filterProfiles, updateProfiles);

            var updateJobOffer = Builders<JobOffer>.Update
                .Set(x => x.CreatedBy.Name, "Removed user")
                .Set(x => x.CreatedBy.Surname, "")
                .Set(x => x.CreatedBy.Avatar, "");
            await _jobOfferRepository.UpdateManyAsync(filterJobOffers, updateJobOffer);
        }
    }
}
