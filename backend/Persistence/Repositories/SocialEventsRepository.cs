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

    public async Task<SocialEvent?> GetById(Guid id)
    {
        var result = await _dbContext.SocialEvents.FirstOrDefaultAsync(e => e.Id == id);

        return result;
    }

    public async Task<SocialEvent> Add(SocialEvent socialEvent)
    {
        var result = await _dbContext.SocialEvents.AddAsync(socialEvent);
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<bool> Update(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Update(socialEvent);

        try
        {
            var result = await _dbContext.SaveChangesAsync();
            
            if (result < 0)
            {
                // TODO: better error text?
                throw new DbUpdateException("Update operation failed!");
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<bool> Delete(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Remove(socialEvent);

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