using DiaryScheduler.Presentation.Services.Scheduler;
using DiaryScheduler.Presentation.Services.Utility;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiaryScheduler.Presentation.Services.Tests.Scheduler
{
    /// <summary>
    /// A collection of tests for the <see cref="SchedulerPresentationService"/>.
    /// </summary>
    [TestFixture]
    public class SchedulerPresentationServiceTests
    {
        private Mock<IScheduleRepository> _scheduleRepo;
        private Mock<IDateTimeService> _dateTimeService;
        private SchedulerPresentationService _schedulerPresentationService;

        [SetUp]
        public void SetUp()
        {
            _scheduleRepo = new Mock<IScheduleRepository>();
            _dateTimeService = new Mock<IDateTimeService>();
            _schedulerPresentationService = new SchedulerPresentationService(
                _scheduleRepo.Object, _dateTimeService.Object);
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
            result.DateFrom.ShouldBe(expectedFromDate);
            result.DateTo.ShouldBe(expectedToDate);
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
            result.Title.ShouldBe(expectedTitle);
            result.DateFrom.ShouldBe(expectedFromDate);
            result.DateTo.ShouldBe(expectedToDate);
        }

        [Test]
        public void CreateSchedulerEditViewModel_GivenInvalidGuid_ReturnsNull()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _scheduleRepo.Setup(x => x.GetCalendarEventByEventId(eventId)).Returns(() => null);

            // Act
            var result = _schedulerPresentationService.CreateSchedulerEditViewModel(eventId);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GetCalendarEventsBetweenDateRange_GivenRequiredParameters_ReturnsConvertedEventCollection()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1, 12, 15, 0);
            var end = new DateTime(2022, 3, 3, 12, 30, 0);
            var userEvents = new List<CalEventDm>()
            {
                new CalEventDm()
                {
                    Title = "Event 1",
                    DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                    DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                    CalendarEntryId = Guid.NewGuid(),
                    AllDay = true
                },
                new CalEventDm()
                {
                    Title = "Event 2",
                    DateFrom = new DateTime(2022, 2, 2, 12, 15, 0),
                    DateTo = new DateTime(2022, 2, 2, 12, 30, 0),
                    CalendarEntryId = Guid.NewGuid(),
                    AllDay = true
                },
                new CalEventDm()
                {
                    Title = "Event 3",
                    DateFrom = new DateTime(2022, 3, 3, 12, 15, 0),
                    DateTo = new DateTime(2022, 3, 3, 12, 30, 0),
                    CalendarEntryId = Guid.NewGuid(),
                    AllDay = false
                }
            };
            var expectedResult = userEvents.Select(x => new
            {
                title = x.Title,
                start = x.DateFrom.ToString("o"),
                end = x.DateTo.ToString("o"),
                id = x.CalendarEntryId,
                allDay = x.AllDay
            });
            var convertedExpectedResult = JsonConvert.SerializeObject(expectedResult);
            _scheduleRepo.Setup(x => x.GetAllEventsBetweenDateRange(start, end)).Returns(userEvents);

            // Act
            var result = _schedulerPresentationService.GetCalendarEventsBetweenDateRange(start, end);
            // Convert the result to json so we can compare the contents match.
            var convertedResult = JsonConvert.SerializeObject(result);

            // Assert
            convertedResult.ShouldBe(convertedExpectedResult);
        }

        [Test]
        public void GenerateIcalForCalendarEvent_GivenInvalidEventId_ReturnsNull()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _scheduleRepo.Setup(x => x.GetCalendarEventByEventId(eventId)).Returns(() => null);

            // Act
            var result = _schedulerPresentationService.GenerateIcalForCalendarEvent(eventId);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GenerateIcalForCalendarEvent_GivenValidEventId_ReturnsModelWithIcalData()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventData = new CalEventDm()
            {
                CalendarEntryId = eventId,
                Title = "Event 1",
                Description = "This is a test event.",
                DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                AllDay = false
            };
            _scheduleRepo.Setup(x => x.GetCalendarEventByEventId(eventId)).Returns(eventData);
            var expectedContentType = "text/calendar";

            // Act
            var result = _schedulerPresentationService.GenerateIcalForCalendarEvent(eventId);

            // Assert
            result.ContentType.ShouldBe(expectedContentType);
            result.Data.ShouldNotBeNull();
            result.FileName.Contains(".ics").ShouldBeTrue();
        }

        [Test]
        public void GenerateIcalBetweenDateRange_GivenInvalidParameters_ReturnsNull()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1, 12, 15, 0);
            var end = new DateTime(2022, 1, 1, 12, 30, 0);
            _scheduleRepo.Setup(x => x.GetAllEventsBetweenDateRange(start, end)).Returns(() => null);

            // Act
            var result = _schedulerPresentationService.GenerateIcalBetweenDateRange(start, end);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GenerateIcalBetweenDateRange_GivenValidParameters_ReturnsModelWithIcalData()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1, 12, 15, 0);
            var end = new DateTime(2022, 1, 1, 12, 30, 0);
            var eventData = new List<CalEventDm>()
            {
                new CalEventDm()
                {
                    CalendarEntryId = Guid.NewGuid(),
                    Title = "Event 1",
                    Description = "This is a test event.",
                    DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                    DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                    AllDay = false
                },
                new CalEventDm()
                {
                    CalendarEntryId = Guid.NewGuid(),
                    Title = "Event 2",
                    Description = "This is a test event.",
                    DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                    DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                    AllDay = false
                },
                new CalEventDm()
                {
                    CalendarEntryId = Guid.NewGuid(),
                    Title = "Event 3",
                    Description = "This is a test event.",
                    DateFrom = new DateTime(2022, 1, 1, 12, 15, 0),
                    DateTo = new DateTime(2022, 1, 1, 12, 30, 0),
                    AllDay = false
                }
            };
            var expectedContentType = "text/calendar";
            _scheduleRepo.Setup(x => x.GetAllEventsBetweenDateRange(start, end)).Returns(eventData);

            // Act
            var result = _schedulerPresentationService.GenerateIcalBetweenDateRange(start, end);

            // Assert
            result.ContentType.ShouldBe(expectedContentType);
            result.Data.ShouldNotBeNull();
            result.FileName.Contains(".ics").ShouldBeTrue();
        }
    }
}
