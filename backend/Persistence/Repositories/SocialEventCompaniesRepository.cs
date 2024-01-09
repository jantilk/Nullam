using Application.DTOs.Requests;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SocialEventCompaniesRepository : ISocialEventCompaniesRepository
{
    private readonly NullamDbContext _dbContext;

    public SocialEventCompaniesRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Guid socialEventId, Guid companyId, AddSocialEventCompanyRequest request)
    {
        var meetupCompany = new SocialEventCompany
        {
            SocialEventId = socialEventId,
            CompanyId = companyId,
            PaymentType = request.PaymentType,
            AdditionalInfo = request.AdditionalInfo,
            CreatedAt = DateTime.Now,
            NumberOfParticipants = request.NumberOfParticipants,
        };

        await _dbContext.SocialEventCompanies.AddAsync(meetupCompany);
    }

    public async Task<List<SocialEventCompany>> GetCompaniesBySocialEventId(Guid eventId)
    {
        var result = await _dbContext.SocialEventCompanies
            .Include(x => x.Company)
            .Where(x => x.SocialEventId == eventId)
            .ToListAsync();

        return result;
    }

    public async Task<SocialEventCompany?> GetByCompanyId(Guid socialEventId, Guid companyId)
    {
        var result = await _dbContext.SocialEventCompanies
            .Include(x => x.Company)
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.CompanyId == companyId)
            .FirstOrDefaultAsync();

        return result;
    }
    
    public async Task<SocialEventCompany?> GetSocialEventCompany(Guid socialEventId, Guid companyId)
    {
        var result = await _dbContext.SocialEventCompanies
            .Include(x => x.SocialEvent)
            .Include(x => x.Company)
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.CompanyId == companyId)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> Update(SocialEventCompany updatedSocialEventCompany)
    {
        _dbContext.SocialEventCompanies.Update(updatedSocialEventCompany);
        
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