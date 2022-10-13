using DiaryScheduler.Api.Integration.Tests.Configuration;
using DiaryScheduler.Presentation.Models.Scheduler;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using System.Collections.Generic;

namespace DiaryScheduler.Api.Integration.Tests.Api.EventManagement;

public class EventManagementControllerGetEventsByDateRangeTests
{
    private ApiWebApplicationFactory _factory;
    private HttpClient _client;
    private string apiUrl = $"api/event-management/events?start={DateTime.UtcNow.AddDays(-3).ToString("yyyy-MM-dd HH:mm")}&end={DateTime.UtcNow.AddDays(3).ToString("yyyy-MM-dd HH:mm")}";

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
    public async Task GetEventById_WithValidRequest_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync(apiUrl);
        var result = await response.Content.ReadAsStringAsync();
        var eventVms = JsonConvert.DeserializeObject<List<CalendarEventViewModel>>(result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        eventVms.Should().NotBeNullOrEmpty();
        eventVms.Count.Should().Be(1);
    }
}
