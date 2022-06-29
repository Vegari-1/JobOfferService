using AutoMapper;
using JobOfferService.Dto;
using JobOfferService.Model;
using JobOfferService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobOfferService
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : Controller
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly IMapper _mapper;

        public JobOfferController(IJobOfferService jobOfferService, IMapper mapper)
        {
            _jobOfferService = jobOfferService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<JobOffer>), 200)]
        public async Task<IActionResult> Filter()
        {
            var jobOfferList = await _jobOfferService.Filter();
            var response = _mapper.Map<ICollection<JobOfferResponse>>(jobOfferList);
            return Ok(response);
        }

        // Just for example purposes
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ICollection<JobOffer>), 200)]
        public async Task<IActionResult> Publish(JobOfferResponse JobOfferResponse)
        {
            var jobOffer = await _jobOfferService.PublishToQue(JobOfferResponse.Id);
            return Ok(_mapper.Map<JobOfferResponse>(jobOffer));
        }
    }
}

