﻿using BusService;
using BusService.Contracts;

using JobOfferService.Model;
using JobOfferService.Service.Interface;

namespace JobOfferService.Service;

public class JobOfferSyncService : ConsumerBase<JobOffer, JobOfferContract>, IJobOfferSyncService
{
    private readonly IMessageBusService _messageBusService;

    public JobOfferSyncService(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }

    public override Task PublishAsync(JobOffer entity, string action)
    {
        throw new NotImplementedException();
    }

    public override Task SynchronizeAsync(JobOfferContract entity, string action)
    {
        throw new NotImplementedException();
    }
}
