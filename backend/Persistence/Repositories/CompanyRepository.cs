using Application.DTOs;
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

    public async Task<List<Company>> Get(FilterDto? filter)
    {
        var query = _dbContext.Companies.AsQueryable();
        
        if (filter != null && !string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTermLower)
                                     || p.RegisterCode.ToString().Contains(searchTermLower));
        }
        
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<Company?> GetById(Guid companyId)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
    }
    
    public async Task<Company?> GetByRegisterCode(int registerCode)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(x => x.RegisterCode == registerCode);
    }

    public async Task<bool> Update(Company updatedCompany)
    {
        _dbContext.Companies.Update(updatedCompany);
        
        var result = await _dbContext.SaveChangesAsync();
        if (result < 0)
        {
            throw new DbUpdateException("Update operation failed!");
        }
        
        return result > 0;
    }
}