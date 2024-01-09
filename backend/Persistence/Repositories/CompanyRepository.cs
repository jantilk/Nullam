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

    public async Task<bool> Update(Company updatedCompany)
    {
        _dbContext.Companies.Update(updatedCompany);
        
        try
        {
            var result = await _dbContext.SaveChangesAsync();
            if (result < 0)
            {
                throw new DbUpdateException("Update operation failed!");
            }
            
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}