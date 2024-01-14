using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace UnitTests.Services;

public class SocialEventPersonsServiceTests
{
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly Mock<ISocialEventPersonsRepository> _mockSocialEventPersonsRepository;
    private readonly Mock<IResourceRepository> _mockResourceRepository;
    private readonly SocialEventPersonsService _service;

    public SocialEventPersonsServiceTests()
    {
        _mockTransactionService = new Mock<ITransactionService>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _mockSocialEventPersonsRepository = new Mock<ISocialEventPersonsRepository>();
        _mockResourceRepository = new Mock<IResourceRepository>();
        _service = new SocialEventPersonsService(_mockTransactionService.Object, _mockPersonRepository.Object, _mockSocialEventPersonsRepository.Object, _mockResourceRepository.Object);
    }
    
    [Fact]
    public async Task Update_SocialEventPersonNotFound()
    {
        // Arrange
        _mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventPerson(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((SocialEventPerson)null);

        // Act
        var result = await _service.Update(Guid.NewGuid(), Guid.NewGuid(), new UpdateSocialEventPersonRequest
        {
            FirstName = "First Name",
            LastName = "Last",
            IdCode = "32409186020",
            PaymentTypeId = default
        });

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Social event person not found", result.Error);
    }
    
    [Fact]
    public async Task Update_PersonNotFound()
    {
        // Arrange
        var existingSocialEventPerson = new SocialEventPerson
        {
            CreatedAt = DateTime.UtcNow,
            ResourceId = Guid.NewGuid(),
            SocialEventId = Guid.NewGuid(),
            PersonId = Guid.NewGuid(),
        };
        _mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventPerson(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(existingSocialEventPerson);
        _mockPersonRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Person)null);

        // Act
        var result = await _service.Update(Guid.NewGuid(), Guid.NewGuid(), new UpdateSocialEventPersonRequest
        {
            FirstName = "updated name",
            LastName = "last",
            IdCode = "60012063706",
            PaymentTypeId = Guid.NewGuid()
        });
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Person not found", result.Error);
    }
    
    [Fact]
    public async Task Update_PaymentTypeNotFound()
    {
        // Arrange
        var existingPerson = new Person
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            FirstName = "exist first",
            LastName = "exist last",
            IdCode = "60012063706"
        };
        var existingSocialEventPerson = new SocialEventPerson
        {
            CreatedAt = DateTime.UtcNow,
            ResourceId = Guid.NewGuid(),
            SocialEventId = Guid.NewGuid(),
            PersonId = existingPerson.Id,
        };

        _mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventPerson(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(existingSocialEventPerson);
        _mockPersonRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(existingPerson);
        _mockResourceRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Resource)null);

        // Act
        var result = await _service.Update(Guid.NewGuid(), Guid.NewGuid(), new UpdateSocialEventPersonRequest
        {
            FirstName = "up first name",
            LastName = "up last",
            IdCode = "60012063706",
            PaymentTypeId = Guid.NewGuid(),
            AdditionalInfo = "lisainfo"
        });

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("PaymentType not found", result.Error);
    }
    
    [Fact]
    public async Task Update_Successful()
    {
        // Arrange
        var existingPerson = new Person
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            FirstName = "exist first",
            LastName = "exist last",
            IdCode = "60012063706"
        };
        var existingSocialEventPerson = new SocialEventPerson
        {
            CreatedAt = DateTime.UtcNow,
            ResourceId = Guid.NewGuid(),
            SocialEventId = Guid.NewGuid(),
            PersonId = existingPerson.Id,
        };
        var existingPaymentType = new Resource
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Type = "PaymentType",
            Text = "Sularahas"
        };
        
        _mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventPerson(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(existingSocialEventPerson);
        _mockPersonRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(existingPerson);
        _mockResourceRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(existingPaymentType);
        _mockPersonRepository.Setup(repo => repo.Update(It.IsAny<Person>())).ReturnsAsync(true);
        _mockSocialEventPersonsRepository.Setup(repo => repo.Update(It.IsAny<SocialEventPerson>())).ReturnsAsync(true);
        var request = new UpdateSocialEventPersonRequest
        {
            FirstName = "up first name",
            LastName = "up last",
            IdCode = "60012063706",
            PaymentTypeId = Guid.NewGuid(),
            AdditionalInfo = "lisainfo"
        };

        // Act
        var result = await _service.Update(Guid.NewGuid(), Guid.NewGuid(), request);

        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task Update_ThrowsException()
    {
        // Arrange
        _mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventPerson(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _service.Update(Guid.NewGuid(), Guid.NewGuid(), new UpdateSocialEventPersonRequest
        {
            FirstName = "update first",
            LastName = "update last",
            IdCode = "42512040152",
            PaymentTypeId = Guid.NewGuid(),
        });

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Update operation failed.", result.Error);
    }
    
    [Fact]
    public async Task Delete_ExistingSocialEventPerson_DeletesSuccessfully()
    {
        // Arrange
        var mockSocialEventPersonsRepository = new Mock<ISocialEventPersonsRepository>();
        mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventsByPersonId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new SocialEventPerson
        {
            CreatedAt = DateTime.UtcNow,
            ResourceId = Guid.NewGuid(),
            SocialEventId = Guid.NewGuid(),
            PersonId = Guid.NewGuid()
        });
        mockSocialEventPersonsRepository.Setup(repo => repo.Delete(It.IsAny<SocialEventPerson>())).ReturnsAsync(true);

        var service = new SocialEventPersonsService(null, null, mockSocialEventPersonsRepository.Object, null);

        // Act
        var result = await service.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_SocialEventPersonNotFound_ReturnsFailure()
    {
        // Arrange
        var mockSocialEventPersonsRepository = new Mock<ISocialEventPersonsRepository>();
        mockSocialEventPersonsRepository
            .Setup(repo => repo.GetSocialEventsByPersonId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((SocialEventPerson)null);

        var service = new SocialEventPersonsService(null, null, mockSocialEventPersonsRepository.Object, null);

        // Act
        var result = await service.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Delete operation failed. Social event person not found", result.Error);
    }

    [Fact]
    public async Task Delete_RepositoryDeleteFails_ReturnsFailure()
    {
        // Arrange
        var mockSocialEventPersonsRepository = new Mock<ISocialEventPersonsRepository>();
        mockSocialEventPersonsRepository.Setup(repo => repo.GetSocialEventsByPersonId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new SocialEventPerson 
            {
                CreatedAt = DateTime.UtcNow,
                ResourceId = Guid.NewGuid(),
                SocialEventId = Guid.NewGuid(),
                PersonId = Guid.NewGuid()
            });
        mockSocialEventPersonsRepository.Setup(repo => repo.Delete(It.IsAny<SocialEventPerson>())).ReturnsAsync(false);

        var service = new SocialEventPersonsService(null, null, mockSocialEventPersonsRepository.Object, null);

        // Act
        var result = await service.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Delete operation failed.", result.Error);
    }
    
    [Fact]
    public async Task Delete_ThrowsException_ReturnsFailure()
    {
        // Arrange
        var mockSocialEventPersonsRepository = new Mock<ISocialEventPersonsRepository>();
        mockSocialEventPersonsRepository
            .Setup(repo => repo.GetSocialEventsByPersonId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Database error"));

        var service = new SocialEventPersonsService(null, null, mockSocialEventPersonsRepository.Object, null);

        // Act
        var result = await service.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Delete operation failed.", result.Error);
    }
}