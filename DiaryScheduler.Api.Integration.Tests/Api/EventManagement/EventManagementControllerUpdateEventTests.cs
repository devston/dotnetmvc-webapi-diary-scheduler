using DiaryScheduler.Api.Integration.Tests.Configuration;
using DiaryScheduler.Presentation.Models.Base;
using DiaryScheduler.Tests.Utilities.ModelBuilders;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using FluentAssertions;

namespace DiaryScheduler.Api.Integration.Tests.Api.EventManagement;

public class EventManagementControllerUpdateEventTests
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
    public async Task UpdateEvent_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = DbTestModels.Event1;
        request.Title = "My new test title";

        // Act
        var response = await _client.PutAsJsonAsync(apiUrl, request);
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task UpdateEvent_WithNonExistantEventId_ReturnsErrors()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();
        string testUrl = $"api/event-management/events/{Guid.NewGuid()}";

        // Act
        var response = await _client.PutAsJsonAsync(testUrl, request);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<BaseResponseViewModel>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        badRequestResponse.Message.Should().Be("The calendar event could not be found.");
    }

    [Test]
    public async Task UpdateEvent_WithMinDateFrom_ReturnsErrors()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();
        request.DateFrom = DateTime.MinValue;

        // Act
        var response = await _client.PutAsJsonAsync(apiUrl, request);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<List<ValidationResultViewModel>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.Count.Should().Be(1);
        badRequestResponse[0].PropertyName.Should().Be(nameof(request.DateFrom));
    }

    [Test]
    public async Task UpdateEvent_WithDateToBeforeDateFrom_ReturnsErrors()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();
        request.DateFrom = DateTime.UtcNow;
        request.DateTo = DateTime.UtcNow.AddDays(-1);

        // Act
        var response = await _client.PutAsJsonAsync(apiUrl, request);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<List<ValidationResultViewModel>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.Count.Should().Be(1);
        badRequestResponse[0].PropertyName.Should().Be(nameof(request.DateTo));
    }
}
