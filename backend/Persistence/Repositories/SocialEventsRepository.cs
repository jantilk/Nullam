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
}