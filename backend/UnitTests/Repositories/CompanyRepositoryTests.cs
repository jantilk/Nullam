using Application.DTOs;
using Application.DTOs.Requests;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace UnitTests.Repositories;

public class CompanyRepositoryTests : TestBase
{
    [Fact]
    public async Task GetCompanies_NoFilter_ReturnsAll()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.Get(null);

        // Assert
        Assert.Equal(companies.Count, result.Count);
    }
    
    [Fact]
    public async Task GetCompanies_FilterByName_ReturnsFilteredCompanies()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        
        var filter = new FilterDto { SearchTerm = "Foods" };
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.Get(filter);

        // Assert
        Assert.Single(result);
        Assert.Equal("L?una Foods", result[0].Name);
    }
    
    [Fact]
    public async Task GetCompanies_FilterByRegisterCode_ReturnsFilteredCompanies()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();
        
        var filter = new FilterDto { SearchTerm = "234" };
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.Get(filter);

        // Assert
        Assert.Contains(result, item => item.RegisterCode == 12345678);
        Assert.Contains(result, item => item.RegisterCode == 23456789);
    }
    
    [Fact]
    public async Task GetCompanies_FilterByTermNotExisting_ReturnsEmptyList()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();
        
        var filter = new FilterDto { SearchTerm = "seda pole" };
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.Get(filter);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetCompanies_ReturnsAll()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.Get((FilterDto)null);

        // Assert
        Assert.Equal(companies.Count, result.Count);
    }
    
    [Fact]
    public async Task GetCompanyById_IdExists_ReturnsOne()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var id = Guid.NewGuid();
        
        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = id, CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public async Task GetCompanyById_IdDoesNotExist_ReturnsNull()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23456789 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        var id = Guid.NewGuid();
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.GetById(id);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetCompanyByRegCode_IdExists_ReturnsOne()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var registerCode = 78541268;
        
        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = registerCode },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.GetByRegisterCode(registerCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(registerCode, result.RegisterCode);
    }
    
    [Fact]
    public async Task GetCompanyByRegCode_RegCodeDoesNotExist_ReturnsNull()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();
        
        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "P?hja Tech", RegisterCode = 12345678 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "L?una Foods", RegisterCode = 23457890 },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
        };

        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();

        var registerCode = 78541268;
        var sut = new CompanyRepository(dbContext);

        // Act
        var result = await sut.GetByRegisterCode(registerCode);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateCompany_ExistingCompany_ReturnsTrue()
    {
        // Arrange
        await using var dbContext = DbContextFactory.CreateDbContext();

        var originalCompany = new Company {Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Original Company", RegisterCode = 12345678};

        await dbContext.Companies.AddAsync(originalCompany);
        await dbContext.SaveChangesAsync();

        var updatedCompany = new Company
        {
            Id = originalCompany.Id,
            CreatedAt = originalCompany.CreatedAt,
            Name = "Updated Company",
            RegisterCode = 98765432
        };

        var sut = new CompanyRepository(dbContext);

        // Act
        dbContext.Entry(originalCompany).State = EntityState.Detached;
        var result = await sut.Update(updatedCompany);
        
        // Assert
        Assert.True(result);

        var updatedCompanyFromDb = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == updatedCompany.Id);

        Assert.NotNull(updatedCompanyFromDb);
        Assert.Equal(updatedCompany.Name, updatedCompanyFromDb.Name);
        Assert.Equal(updatedCompany.RegisterCode, updatedCompanyFromDb.RegisterCode);
    }
}