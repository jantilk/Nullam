using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests.Services;

public class DeleteSocialEventsTests : TestBase
{
    [Fact]
    public async Task Delete_ExistingFutureEvent_ReturnsSuccess()
    {
        // Arrange
        var mockRepository = new Mock<ISocialEventsRepository>();
        var futureEvent = new SocialEvent
        {
            Id = Guid.NewGuid(),
            CreatedAt = default,
            Name = "Future event",
            Date = DateTime.UtcNow.AddDays(1),
            Location = "Tallinn",
            AdditionalInfo = "Future info"
        };
        mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(futureEvent);
        mockRepository.Setup(repo => repo.Delete(futureEvent)).ReturnsAsync(true);

        var service = new SocialEventsService(mockRepository.Object);

        // Act
        var result = await service.Delete(Guid.NewGuid());

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_NonExistentEvent_ReturnsFailure()
    {
        // Arrange
        var mockRepository = new Mock<ISocialEventsRepository>();
        mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((SocialEvent)null);

        var service = new SocialEventsService(mockRepository.Object);

        // Act
        var result = await service.Delete(Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Delete operation failed. Social event not found.", result.Error);
    }

    [Fact]
    public async Task Delete_PastEvent_ReturnsFailure()
    {
        // Arrange
        var mockRepository = new Mock<ISocialEventsRepository>();
        var pastEvent = new SocialEvent
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = "Past event",
            Date = DateTime.UtcNow.AddDays(-1),
            Location = "P?rnu",
            AdditionalInfo = "Additional past info"

        };
        mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(pastEvent);

        var service = new SocialEventsService(mockRepository.Object);

        // Act
        var result = await service.Delete(Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Delete operation failed. Cannot delete past event.", result.Error);
    }
    
    [Fact]
    public async Task Delete_RepositoryDeleteFails_ReturnsFailure()
    {
        // Arrange
        var mockRepository = new Mock<ISocialEventsRepository>();
        var futureEvent = new SocialEvent
        {
            Id = default,
            CreatedAt = default,
            Name = "Delete will fail",
            Date = DateTime.UtcNow.AddDays(1),
            Location = "Tallinn",

        };
        mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(futureEvent);
        mockRepository.Setup(repo => repo.Delete(futureEvent)).ReturnsAsync(false);

        var service = new SocialEventsService(mockRepository.Object);

        // Act
        var result = await service.Delete(Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Delete operation failed.", result.Error);
    }

    [Fact]
    public async Task Delete_ThrowsException_ReturnsFailure()
    {
        // Arrange
        var mockRepository = new Mock<ISocialEventsRepository>();
        mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

        var service = new SocialEventsService(mockRepository.Object);

        // Act
        var result = await service.Delete(Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Delete operation failed.", result.Error);
    }
}