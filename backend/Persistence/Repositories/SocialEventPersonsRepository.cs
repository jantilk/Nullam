using Application.DTOs.Requests;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SocialEventPersonsRepository : ISocialEventPersonsRepository
{
    private readonly NullamDbContext _dbContext;

    public SocialEventPersonsRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Guid socialEventId, Guid personId, AddSocialEventPersonRequest request)
    {
        var socialEventPerson = new SocialEventPerson
        {
            SocialEventId = socialEventId,
            PersonId = personId,
            ResourceId = request.PaymentTypeId,
            AdditionalInfo = request.AdditionalInfo,
            CreatedAt = DateTime.Now,
        };

        await _dbContext.SocialEventPersons.AddAsync(socialEventPerson);
    }

    public async Task<List<SocialEventPerson>> GetBySocialEventId(Guid socialEventId)
    {
        var result = await _dbContext.SocialEventPersons
            .Include(x => x.Person)
            .Where(x => x.SocialEventId == socialEventId)
            .ToListAsync();

        return result;
    }

    public async Task<SocialEventPerson?> GetByPersonId(Guid socialEventId, Guid personId)
    {
        var result = await _dbContext.SocialEventPersons
            .Include(x => x.Person)
            .Include(x => x.SocialEvent)
            .Include(x => x.PaymentType)
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.PersonId == personId)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<SocialEventPerson?> GetSocialEventPerson(Guid socialEventId, Guid personId)
    {
        var result = await _dbContext.SocialEventPersons
            .Include(x => x.Person)
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.PersonId == personId)
            .FirstOrDefaultAsync();

        return result;
    }
    
    public async Task<List<SocialEventPerson>> GetByResourceId(Guid resourceId)
    {
        return await _dbContext.SocialEventPersons.Where(x => x.ResourceId == resourceId).ToListAsync();
    }

    public async Task<bool> Update(SocialEventPerson updatedSocialEventPerson)
    {
        _dbContext.SocialEventPersons.Update(updatedSocialEventPerson);
        

        var result = await _dbContext.SaveChangesAsync();
        if (result < 0)
        {
            throw new DbUpdateException("Update operation failed.");
        }
        
        return result > 0;
    }

    public async Task<bool> Delete(SocialEventPerson socialEventPerson)
    {
        _dbContext.SocialEventPersons.Remove(socialEventPerson);

        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}