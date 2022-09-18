using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Xunit;

using JobOfferService.Dto;
using JobOfferService.Repository.Interface.Pagination;
using BusService.Contracts;
using System;

namespace JobOfferService.IntegrationTests;

public class JobOfferControllerTests : IClassFixture<IntegrationWebApplicationFactory<Program>>
{
    private readonly IntegrationWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    private static readonly string positionName = "Test Position";
    private static readonly string description = "Position description";
    private static readonly string companyLink = "https://www.youtube.com/";
    private static readonly string[] qualifications = new[] { "first", "second", "third" };

    private static readonly Guid authorGlobalId = Guid.NewGuid();
    private static readonly string authorName = "Author";
    private static readonly string authorSurname = "Creator";
    private static readonly string authorAvatar = "Avatar:asdaswe23r";
    private static readonly string authorEmail = "author@complex.com";
    private static readonly string authorUsername = "author";


    public JobOfferControllerTests(IntegrationWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("profile-id", authorGlobalId.ToString());
    }

    [Fact]
    public async Task GetJobOffers_CorrectData_CollectionJobOfferResponse()
    {
        // Given
        string jobOfferId = _factory.InsertJobOffer(positionName, description, companyLink,
            qualifications, authorName, authorSurname, authorGlobalId);

        // When
        var response = await _client.GetAsync("/api/joboffer");

        // Then
        response.EnsureSuccessStatusCode();
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject = JsonConvert.DeserializeObject<PagedList<JobOfferResponse>>(responseContentString);

        Assert.NotNull(responseContentObject);
        Assert.Equal(1, responseContentObject?.Items.Count);

        Assert.Equal(positionName, responseContentObject?.Items.First().PositionName);
        Assert.Equal(description, responseContentObject?.Items.First().Description);
        Assert.Equal(companyLink, responseContentObject?.Items.First().CompanyLink);
        Assert.Equal(qualifications, responseContentObject?.Items.First().Qualifications);

        // Rollback
        _factory.DeleteJobOfferById(jobOfferId);
    }

    [Fact]
    public async Task PostJobOfferRequest_CorrectData_JobOfferResponse()
    {
        // Given
        JobOfferPostRequest jobOffer = new()
        {
            PositionName = positionName,
            Description = description,
            CompanyLink = companyLink,
            Qualifications = qualifications
        };
        var requestContent = new StringContent(JsonConvert.SerializeObject(jobOffer), Encoding.UTF8, "application/json");

        // When
        _factory.InsertProfile(authorName, authorSurname, authorGlobalId);
        var response = await _client.PostAsync("/api/joboffer", requestContent);

        // Then
        response.EnsureSuccessStatusCode();
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject = JsonConvert.DeserializeObject<JobOfferResponse>(responseContentString);

        Assert.NotNull(responseContentObject);
        Assert.NotNull(responseContentObject?.Id);

        Assert.Equal(positionName, responseContentObject?.PositionName);
        Assert.Equal(description, responseContentObject?.Description);
        Assert.Equal(companyLink, responseContentObject?.CompanyLink);
        Assert.Equal(qualifications, responseContentObject?.Qualifications);

        Assert.Equal(authorName, responseContentObject?.Profile?.Name);
        Assert.Equal(authorSurname, responseContentObject?.Profile?.Surname);
        Assert.Equal(authorGlobalId.ToString(), responseContentObject?.Profile?.GlobalId);

        // Rollback
        _factory.DeleteJobOfferById(responseContentObject.Id);
    }


    // TODO: we should move this to separate test class
    [Fact]
    public void JobOfferSyncService_ReceiveUpdate_JobOfferUpdated()
    {
        // Given 
        var newAuthorName = "Decompositor";
        var jobOfferId = _factory.InsertJobOffer(positionName, description, companyLink, qualifications, authorName, authorSurname, authorGlobalId);
        var jobOffer = _factory.GetJobOfferById(jobOfferId);
        var profile = new ProfileContract(jobOffer.CreatedBy.GlobalId, newAuthorName, authorSurname, authorEmail, authorUsername, authorAvatar);

        // When
        _factory.PublishProfileUpdate(profile);

        // Sleep as sync is performed in async way
        System.Threading.Thread.Sleep(3000);

        // Then
        jobOffer = _factory.GetJobOfferById(jobOfferId);
        Assert.Equal(jobOffer.CreatedBy.Name, newAuthorName);

        // Rollback
        _factory.DeleteJobOfferById(jobOfferId);
    }
}
