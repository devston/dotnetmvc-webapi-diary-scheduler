using DiaryScheduler.Api.Integration.Tests.Configuration;
using DiaryScheduler.Presentation.Models.Base;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using FluentAssertions;

namespace DiaryScheduler.Api.Integration.Tests.Api.EventManagement;

public class EventManagementControllerDeleteEventTests
{
    private ApiWebApplicationFactory _factory;
    private HttpClient _client;
    private string apiUrl = $"api/event-management/events/{DbTestModels.Event1.CalendarEventId}";

    [OneTimeSetUp]
    public void SetUp()
    {
        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task DeleteEvent_WithValidRequest_ReturnsSuccess()
    {
        // Act
        var response = await _client.DeleteAsync(apiUrl);
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task DeleteEvent_WithNonExistantEventId_ReturnsErrors()
    {
        // Arrange
        var testUrl = $"api/event-management/events/{Guid.NewGuid()}";

        // Act
        var response = await _client.DeleteAsync(testUrl);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<BaseResponseViewModel>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        badRequestResponse.Message.Should().Be("The calendar event could not be found.");
    }
}
