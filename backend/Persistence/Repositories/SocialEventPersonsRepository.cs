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
            PaymentType = request.PaymentType,
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
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.PersonId == personId)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<SocialEventPerson?> GetSocialEventPerson(Guid socialEventId, Guid personId)
    {
        var result = await _dbContext.SocialEventPersons
            .Include(x => x.PersonId)
            .Where(x => x.SocialEventId == socialEventId)
            .Where(x => x.PersonId == personId)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> Update(SocialEventPerson updatedSocialEventPerson)
    {
        _dbContext.SocialEventPersons.Update(updatedSocialEventPerson);
        
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

    public async Task<bool> Delete(SocialEventPerson socialEventPerson)
    {
        _dbContext.SocialEventPersons.Remove(socialEventPerson);

        try
        {
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            // TODO: better error handling
            // TODO: check elsewhere also
            Console.WriteLine(ex);
            throw;
        }
    }
}