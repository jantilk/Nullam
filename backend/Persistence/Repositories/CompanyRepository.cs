using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly NullamDbContext _dbContext;

    public CompanyRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Company company)
    {
        await _dbContext.Companies.AddAsync(company);
    }

    public async Task<Company?> Get(Guid companyId)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
    }
}