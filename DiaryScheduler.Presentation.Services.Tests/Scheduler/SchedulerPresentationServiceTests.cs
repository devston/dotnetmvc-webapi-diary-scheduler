using DiaryScheduler.Presentation.Models.Scheduler;
using DiaryScheduler.Presentation.Services.Scheduler;
using DiaryScheduler.Presentation.Services.Utility;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Tests.Scheduler;

/// <summary>
/// A collection of tests for the <see cref="SchedulerPresentationService"/>.
/// </summary>
[TestFixture]
public class SchedulerPresentationServiceTests
{
    private Mock<IEventApi> _apiService;
    private Mock<IDateTimeService> _dateTimeService;
    private SchedulerPresentationService _schedulerPresentationService;

    [SetUp]
    public void SetUp()
    {
        _apiService = new Mock<IEventApi>();
        _dateTimeService = new Mock<IDateTimeService>();
        _schedulerPresentationService = new SchedulerPresentationService(
            _apiService.Object, _dateTimeService.Object);
    }

    [Test]
    public void CreateSchedulerCreateViewModel_GivenTimeAs1210_ReturnsDateRangeWith1215And1230()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 1, 12, 10, 0);
        var expectedFromDate = new DateTime(2022, 1, 1, 12, 15, 0);
        var expectedToDate = new DateTime(2022, 1, 1, 12, 30, 0);
        _dateTimeService.Setup(x => x.GetDateTimeUtcNow()).Returns(fromDate);

        // Act
        var result = _schedulerPresentationService.CreateSchedulerCreateViewModel();

        // Assert
        result.DateFrom.Should().Be(expectedFromDate);
        result.DateTo.Should().Be(expectedToDate);
    }

    [Test]
    public void CreateSchedulerCreateViewModel_GivenProvidedParameters_ReturnsModelWithSetParameters()
    {
        // Arrange
        var expectedTitle = "Test";
        var expectedFromDate = new DateTime(2022, 1, 1, 12, 15, 0);
        var expectedToDate = new DateTime(2022, 1, 1, 12, 30, 0);

        // Act
        var result = _schedulerPresentationService.CreateSchedulerCreateViewModel(expectedTitle, expectedFromDate, expectedToDate);

        // Assert
        result.Title.Should().Be(expectedTitle);
        result.DateFrom.Should().Be(expectedFromDate);
        result.DateTo.Should().Be(expectedToDate);
    }

    [Test]
    public async Task CreateSchedulerEditViewModelAsync_GivenInvalidGuid_ReturnsNull()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _apiService.Setup(x => x.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // Act
        var result = await _schedulerPresentationService.CreateSchedulerEditViewModelAsync(eventId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetCalendarEventsBetweenDateRangeAsync_GivenRequiredParameters_ReturnsConvertedEventCollection()
    {
        // Arrange
        var start = new DateTime(2022, 1, 1, 12, 15, 0);
        var end = new DateTime(2022, 3, 3, 12, 30, 0);
        var calendarEvents = new List<CalendarEventViewModel>()
        {
            new CalendarEventViewModel()
            {
                Title = "Event 1",
                DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                CalendarEventId = Guid.NewGuid(),
                AllDay = true
            },
            new CalendarEventViewModel()
            {
                Title = "Event 2",
                DateFrom = new DateTime(2022, 2, 2, 12, 15, 0),
                DateTo = new DateTime(2022, 2, 2, 12, 30, 0),
                CalendarEventId = Guid.NewGuid(),
                AllDay = true
            },
            new CalendarEventViewModel()
            {
                Title = "Event 3",
                DateFrom = new DateTime(2022, 3, 3, 12, 15, 0),
                DateTo = new DateTime(2022, 3, 3, 12, 30, 0),
                CalendarEventId = Guid.NewGuid(),
                AllDay = false
            }
        };
        var expectedResult = calendarEvents.Select(x => new
        {
            title = x.Title,
            start = x.DateFrom.ToString("o"),
            end = x.DateTo.ToString("o"),
            id = x.CalendarEventId,
            allDay = x.AllDay
        });
        var convertedExpectedResult = JsonConvert.SerializeObject(expectedResult);
        _apiService.Setup(x => x.GetEventsBetweenDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(calendarEvents);

        // Act
        var result = await _schedulerPresentationService.GetCalendarEventsBetweenDateRangeAsync(start, end);
        // Convert the result to json so we can compare the contents match.
        var convertedResult = JsonConvert.SerializeObject(result);

        // Assert
        convertedResult.Should().Be(convertedExpectedResult);
    }

    [Test]
    public async Task GenerateIcalForCalendarEventAsync_GivenInvalidEventId_ReturnsNull()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _apiService.Setup(x => x.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // Act
        var result = await _schedulerPresentationService.GenerateIcalForCalendarEventAsync(eventId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GenerateIcalForCalendarEventAsync_GivenValidEventId_ReturnsModelWithIcalData()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventData = new CalendarEventViewModel()
        {
            CalendarEventId = eventId,
            Title = "Event 1",
            Description = "This is a test event.",
            DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
            DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
            AllDay = false
        };
        _apiService.Setup(x => x.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync(eventData);
        var expectedContentType = "text/calendar";

        // Act
        var result = await _schedulerPresentationService.GenerateIcalForCalendarEventAsync(eventId);

        // Assert
        result.ContentType.Should().Be(expectedContentType);
        result.Data.Should().NotBeNull();
        result.FileName.Contains(".ics").Should().BeTrue();
    }

    [Test]
    public async Task GenerateIcalBetweenDateRangeAsync_GivenInvalidParameters_ReturnsNull()
    {
        // Arrange
        var start = new DateTime(2022, 1, 1, 12, 15, 0);
        var end = new DateTime(2022, 1, 1, 12, 30, 0);
        _apiService.Setup(x => x.GetEventsBetweenDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _schedulerPresentationService.GenerateIcalBetweenDateRangeAsync(start, end);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GenerateIcalBetweenDateRangeAsync_GivenValidParameters_ReturnsModelWithIcalData()
    {
        // Arrange
        var start = new DateTime(2022, 1, 1, 12, 15, 0);
        var end = new DateTime(2022, 1, 1, 12, 30, 0);
        var eventData = new List<CalendarEventViewModel>()
        {
            new CalendarEventViewModel()
            {
                CalendarEventId = Guid.NewGuid(),
                Title = "Event 1",
                Description = "This is a test event.",
                DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                AllDay = false
            },
            new CalendarEventViewModel()
            {
                CalendarEventId = Guid.NewGuid(),
                Title = "Event 2",
                Description = "This is a test event.",
                DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                AllDay = false
            },
            new CalendarEventViewModel()
            {
                CalendarEventId = Guid.NewGuid(),
                Title = "Event 3",
                Description = "This is a test event.",
                DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                AllDay = false
            }
        };
        var expectedContentType = "text/calendar";
        _apiService.Setup(x => x.GetEventsBetweenDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(eventData);

        // Act
        var result = await _schedulerPresentationService.GenerateIcalBetweenDateRangeAsync(start, end);

        // Assert
        result.ContentType.Should().Be(expectedContentType);
        result.Data.Should().NotBeNull();
        result.FileName.Contains(".ics").Should().BeTrue();
    }
}
