using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SocialEventsRepository : ISocialEventsRepository
{
    private readonly NullamDbContext _dbContext;

    public SocialEventsRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(SocialEvent socialEvent)
    {
        await _dbContext.SocialEvents.AddAsync(socialEvent);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<SocialEvent>> Get(SortingOption? orderBy, FilterDto? filter)
    {
        var query = _dbContext.SocialEvents.AsQueryable();
        
        if (filter != null)
        {
            if (filter.StartDate.HasValue)
            {
                query = query.Where(e => e.Date >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(e => e.Date <= filter.EndDate.Value);
            }
        }

        if (orderBy != null)
        {
            query = orderBy switch
            {
                SortingOption.DateAsc => query.OrderBy(e => e.Date),
                SortingOption.DateDesc => query.OrderByDescending(e => e.Date),
                _ => query
            };
        }
        
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<SocialEvent?> GetById(Guid socialEventId)
    {
        var result = await _dbContext.SocialEvents.FirstOrDefaultAsync(e => e.Id == socialEventId);

        return result;
    }

    public async Task<bool> Update(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Update(socialEvent);

        var result = await _dbContext.SaveChangesAsync();
        
        if (result < 0)
        {
            throw new DbUpdateException("Update operation failed.");
        }

        return true;
    }

    public async Task<bool> Delete(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Remove(socialEvent);

        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}