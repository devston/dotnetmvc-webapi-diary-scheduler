using DiaryScheduler.Api.Services.EventManagement;
using DiaryScheduler.Presentation.Models.Scheduler;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Services.Tests.EventManagement;

/// <summary>
/// A collection of tests for the <see cref="EventManagementApiService"/>.
/// </summary>
[TestFixture]
public class EventManagementApiServiceTests
{
    private Mock<IScheduleRepository> _scheduleRepository;
    private EventManagementApiService _eventManagementApiService;

    [SetUp]
    public void SetUp()
    {
        _scheduleRepository = new Mock<IScheduleRepository>();
        _eventManagementApiService = new EventManagementApiService(
            _scheduleRepository.Object);
    }

    [Test]
    public async Task GetCalendarEventsBetweenDateRangeAsync_GivenNull_ReturnsEmptyCalendarCollection()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 1, 12, 10, 0);
        var toDate = new DateTime(2022, 1, 1, 12, 30, 0);
        _scheduleRepository.Setup(x => x.GetAllEventsBetweenDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<CalEventDm>());

        // Act
        var result = await _eventManagementApiService.GetCalendarEventsBetweenDateRangeAsync(fromDate, toDate);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        result.Should().BeOfType<List<CalendarEventViewModel>>();
    }

    [Test]
    public async Task GetCalendarEventsBetweenDateRangeAsync_GivenValidDates_ReturnsCalendarCollection()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 1, 12, 10, 0);
        var toDate = new DateTime(2022, 1, 1, 12, 30, 0);
        var calEvent = new CalEventDm()
        {
            Title = "Test"
        };
        _scheduleRepository.Setup(x => x.GetAllEventsBetweenDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<CalEventDm>() { calEvent });

        // Act
        var result = await _eventManagementApiService.GetCalendarEventsBetweenDateRangeAsync(fromDate, toDate);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.Should().BeOfType<List<CalendarEventViewModel>>();
        result.First().Title.Should().Be(calEvent.Title);
    }

    [Test]
    public async Task GetCalendarEventByIdAsync_GivenInvalidId_ReturnsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _scheduleRepository.Setup(x => x.GetCalendarEventByEventIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CalEventDm)null);

        // Act
        Func<Task> act = async () => await _eventManagementApiService.GetCalendarEventByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Test]
    public async Task GetCalendarEventByIdAsync_GivenValidId_ReturnsCalendarEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var calEvent = new CalEventDm()
        {
            CalendarEntryId = id,
            Title = "Test",
            DateFrom = DateTime.UtcNow,
            DateTo = DateTime.UtcNow.AddDays(1),
            Description = "Test description",
            AllDay = true
        };
        _scheduleRepository.Setup(x => x.GetCalendarEventByEventIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(calEvent);

        // Act
        var result = await _eventManagementApiService.GetCalendarEventByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.CalendarEventId.Should().Be(calEvent.CalendarEntryId);
        result.Title.Should().Be(calEvent.Title);
        result.DateFrom.Should().Be(calEvent.DateFrom);
        result.DateTo.Should().Be(calEvent.DateTo);
        result.Description.Should().Be(calEvent.Description);
        result.AllDay.Should().Be(calEvent.AllDay);
    }

    [Test]
    public async Task CreateCalendarEventAsync_GivenValidCalendarEvent_ReturnsCalendarEventId()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var calEvent = new CalendarEventViewModel()
        {
            Title = "Test",
            DateFrom = DateTime.UtcNow,
            DateTo = DateTime.UtcNow.AddDays(1),
            Description = "Test description",
            AllDay = true
        };
        _scheduleRepository.Setup(x => x.CreateCalendarEventAsync(It.IsAny<CalEventDm>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _eventManagementApiService.CreateCalendarEventAsync(calEvent);

        // Assert
        result.Should().Be(expectedId);
    }

    [Test]
    public async Task UpdateCalendarEventAsync_GivenInvalidCalendarEventId_ReturnsException()
    {
        // Arrange
        var calEvent = new CalendarEventViewModel()
        {
            CalendarEventId = Guid.NewGuid(),
            Title = "Test",
            DateFrom = DateTime.UtcNow,
            DateTo = DateTime.UtcNow.AddDays(1),
            Description = "Test description",
            AllDay = true
        };
        _scheduleRepository.Setup(x => x.DoesEventExistAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _eventManagementApiService.UpdateCalendarEventAsync(calEvent);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task UpdateCalendarEventAsync_GivenValidCalendarEventId_ReturnsMessage()
    {
        // Arrange
        var calEvent = new CalendarEventViewModel()
        {
            CalendarEventId = Guid.NewGuid(),
            Title = "Test",
            DateFrom = DateTime.UtcNow,
            DateTo = DateTime.UtcNow.AddDays(1),
            Description = "Test description",
            AllDay = true
        };
        _scheduleRepository.Setup(x => x.DoesEventExistAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _scheduleRepository.Setup(x => x.EditCalendarEventAsync(It.IsAny<CalEventDm>()));

        // Act
        var result = await _eventManagementApiService.UpdateCalendarEventAsync(calEvent);

        // Assert
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task DeleteCalendarEventAsync_GivenInvalidCalendarEventId_ReturnsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _scheduleRepository.Setup(x => x.DoesEventExistAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _eventManagementApiService.DeleteCalendarEventAsync(id);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task DeleteCalendarEventAsync_GivenValidCalendarEventId_ReturnsMessage()
    {
        // Arrange
        var id = Guid.NewGuid();
        _scheduleRepository.Setup(x => x.DoesEventExistAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        _scheduleRepository.Setup(x => x.DeleteCalendarEventAsync(It.IsAny<Guid>()));

        // Act
        var result = await _eventManagementApiService.DeleteCalendarEventAsync(id);

        // Assert
        result.Should().NotBeEmpty();
    }
}
