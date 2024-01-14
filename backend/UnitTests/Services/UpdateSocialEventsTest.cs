using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests.Services;

public class UpdateSocialEventsTest : TestBase
{
    public class UpdateSocialEventTests
    {
        [Fact]
        public async Task Update_ExistingEvent_ReturnsSuccess()
        {
            // Arrange
            var mockRepository = new Mock<ISocialEventsRepository>();
            var socialEvent = new SocialEvent
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = "Original Event",
                Date = DateTime.UtcNow.AddDays(10),
                Location = "Tallinn",
                AdditionalInfo = "Original info"
            };
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(socialEvent);
            mockRepository.Setup(repo => repo.Update(It.IsAny<SocialEvent>())).ReturnsAsync(true);

            var service = new SocialEventsService(mockRepository.Object);
            var request = new UpdateSocialEventRequest
            {
                Name = "Updated event",
                Date = socialEvent.Date.AddMonths(1),
                Location = "P?rnu",
                AdditionalInfo = "Updated info"
            };

            // Act
            var result = await service.Update(Guid.NewGuid(), request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
        
        [Fact]
        public async Task Update_NonExistentEvent_ReturnsFailure()
        {
            // Arrange
            var mockRepository = new Mock<ISocialEventsRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((SocialEvent)null);

            var service = new SocialEventsService(mockRepository.Object);

            // Act
            var result = await service.Update(Guid.NewGuid(), new UpdateSocialEventRequest
            {
                Name = "Updated event",
                Date = DateTime.UtcNow.AddMonths(1),
                Location = "P?rnu",
                AdditionalInfo = "Updated info"
            });

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Update operation failed, social event not found.", result.Error);
        }
        
        [Fact]
        public async Task Update_RepositoryThrowsException_ReturnsFailure()
        {
            // Arrange
            var mockRepository = new Mock<ISocialEventsRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

            var service = new SocialEventsService(mockRepository.Object);

            // Act
            var result = await service.Update(Guid.NewGuid(), new UpdateSocialEventRequest
            {
                Name = "Updated event",
                Date = DateTime.UtcNow.AddMonths(1),
                Location = "P?rnu",
                AdditionalInfo = "Updated info"
            });

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Update operation failed!", result.Error);
        }
        
        [Fact]
        public async Task Update_ExistingEvent_UpdatesFieldsCorrectly()
        {
            // Arrange
            var mockRepository = new Mock<ISocialEventsRepository>();
            var originalEvent = new SocialEvent
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = "Original Event",
                Date = DateTime.UtcNow.AddDays(10),
                Location = "Tallinn",
                AdditionalInfo = "Original info"
            };
            var updateRequest = new UpdateSocialEventRequest
            {
                Name = "Updated event",
                Date = DateTime.UtcNow.AddMonths(1),
                Location = "P?rnu",
                AdditionalInfo = "Updated info"
            };

            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(originalEvent);
            mockRepository.Setup(repo => repo.Update(It.IsAny<SocialEvent>())).Callback<SocialEvent>(e => 
            {
                originalEvent = e;
            });

            var service = new SocialEventsService(mockRepository.Object);

            // Act
            await service.Update(Guid.NewGuid(), updateRequest);

            // Assert
            Assert.Equal(updateRequest.Name, originalEvent.Name);
            Assert.Equal(updateRequest.Date, originalEvent.Date);
            Assert.Equal(updateRequest.Location, originalEvent.Location);
            Assert.Equal(updateRequest.AdditionalInfo, originalEvent.AdditionalInfo);
        }
    }
}