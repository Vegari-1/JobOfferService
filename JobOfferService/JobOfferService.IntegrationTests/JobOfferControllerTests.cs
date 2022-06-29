using JobOfferService.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobOfferService.IntegrationTests
{
    public class JobOfferControllerTests : IClassFixture<IntegrationWebApplicationFactory<Program>>
    {
        private readonly IntegrationWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private static readonly string positionName = "Test Position";
        private static readonly string description = "Position description";

        public JobOfferControllerTests(IntegrationWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ConnectionEstablished()
        {
            // Given
            string jobOfferId = _factory.InsertJobOffer(positionName, description);

            // When
            var response = await _client.GetAsync("/api/joboffer");

            // Then
            response.EnsureSuccessStatusCode();
            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseContentObject = JsonConvert.DeserializeObject<ICollection<JobOfferResponse>>(responseContentString);
            Assert.NotNull(responseContentObject);
            Assert.Equal(1, responseContentObject.Count);
            Assert.Equal(positionName, responseContentObject.First().PositionName);
            Assert.Equal(description, responseContentObject.First().Description);

            // Rollback
            _factory.DeleteJobOfferById(jobOfferId);
        }

        [Fact]
        public async Task Proba()
        {
            // Given
            string jobOfferId = _factory.InsertJobOffer(positionName, description);
            JobOfferResponse jobOffer = new JobOfferResponse()
            {
                Id = jobOfferId,
                PositionName = positionName,
                Description = description
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(jobOffer), Encoding.UTF8, "application/json");

            // When
            var response = await _client.PostAsync("/api/joboffer", requestContent);

            // Then
            response.EnsureSuccessStatusCode();
            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseContentObject = JsonConvert.DeserializeObject<JobOfferResponse>(responseContentString);
            Assert.NotNull(responseContentObject);
            Assert.Equal(jobOfferId, responseContentObject.Id);
            Assert.Equal(positionName, responseContentObject.PositionName);
            Assert.Equal(description, responseContentObject.Description);

            // Rollback
            _factory.DeleteJobOfferById(jobOfferId);
        }
    }
}
