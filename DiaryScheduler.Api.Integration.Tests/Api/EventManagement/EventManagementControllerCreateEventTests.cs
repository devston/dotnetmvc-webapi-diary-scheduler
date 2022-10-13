using DiaryScheduler.Api.Integration.Tests.Configuration;
using DiaryScheduler.Presentation.Models.Base;
using DiaryScheduler.Tests.Utilities.ModelBuilders;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Integration.Tests.Api.EventManagement;

public class EventManagementControllerCreateEventTests
{
    private ApiWebApplicationFactory _factory;
    private HttpClient _client;
    private const string apiUrl = "api/event-management/events";

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
    public async Task CreateEvent_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();

        // Act
        var response = await _client.PostAsJsonAsync(apiUrl, request);
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateEvent_WithMinDateFrom_ReturnsErrors()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();
        request.DateFrom = DateTime.MinValue;

        // Act
        var response = await _client.PostAsJsonAsync(apiUrl, request);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<List<ValidationResultViewModel>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.Count.Should().Be(1);
        badRequestResponse[0].PropertyName.Should().Be(nameof(request.DateFrom));
    }

    [Test]
    public async Task CreateEvent_WithDateToBeforeDateFrom_ReturnsErrors()
    {
        // Arrange
        var request = EventModelBuilder.CreateValidCalendarEventViewModel();
        request.DateFrom = DateTime.UtcNow;
        request.DateTo = DateTime.UtcNow.AddDays(-1);

        // Act
        var response = await _client.PostAsJsonAsync(apiUrl, request);
        var responseString = await response.Content.ReadAsStringAsync();
        var badRequestResponse = JsonConvert.DeserializeObject<List<ValidationResultViewModel>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.Count.Should().Be(1);
        badRequestResponse[0].PropertyName.Should().Be(nameof(request.DateTo));
    }
}
