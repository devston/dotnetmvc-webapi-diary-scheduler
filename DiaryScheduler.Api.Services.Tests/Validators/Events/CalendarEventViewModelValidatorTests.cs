using DiaryScheduler.Api.Services.Validators.Events;
using DiaryScheduler.Tests.Utilities.ModelBuilders;
using DiaryScheduler.Tests.Utilities.StringHelpers;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Services.Tests.Validators.Events;

public class CalendarEventViewModelValidatorTests
{
    private CalendarEventViewModelValidator _eventValidator;

    [SetUp]
    public void SetUp()
    {
        _eventValidator = new CalendarEventViewModelValidator();
    }

    [Test]
    public async Task CalendarEventId_WithEmptyValue_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.CalendarEventId = Guid.Empty;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CalendarEventId);
    }

    [Test]
    public async Task CalendarEventId_WithValue_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.CalendarEventId = Guid.NewGuid();

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CalendarEventId);
    }

    [Test]
    public async Task DateFrom_WithValueBeforeDateTo_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.DateFrom = DateTime.UtcNow;
        eventVm.DateTo = DateTime.UtcNow.AddDays(1);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DateFrom);
    }

    [Test]
    public async Task DateFrom_WithMinDate_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.DateFrom = DateTime.MinValue;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateFrom);
    }

    [Test]
    public async Task DateTo_WithValueAfterDateFrom_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.DateFrom = DateTime.UtcNow;
        eventVm.DateTo = DateTime.UtcNow.AddDays(1);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DateTo);
    }

    [Test]
    public async Task DateTo_WithValueBeforeDateFrom_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.DateFrom = DateTime.UtcNow;
        eventVm.DateTo = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateTo);
    }

    [Test]
    public async Task Title_With100CharacterString_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Title = StringGenerator.GenerateRandomAlphanumericString(100);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public async Task Title_WithNullValue_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Title = null;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public async Task Title_WithEmptyValue_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Title = string.Empty;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public async Task Title_With101CharacterString_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Title = StringGenerator.GenerateRandomAlphanumericString(101);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public async Task Description_With200CharacterString_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Description = StringGenerator.GenerateRandomAlphanumericString(200);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public async Task Description_WithNullValue_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Description = null;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public async Task Description_WithEmptyValue_ReturnsNoErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Description = string.Empty;

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public async Task Description_With201CharacterString_ReturnsErrors()
    {
        // Arrange
        var eventVm = EventModelBuilder.CreateValidCalendarEventViewModel();
        eventVm.Description = StringGenerator.GenerateRandomAlphanumericString(201);

        // Act
        var result = await _eventValidator.TestValidateAsync(eventVm);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}
