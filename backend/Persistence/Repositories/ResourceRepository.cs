using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly NullamDbContext _dbContext;

    public ResourceRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Resource resource)
    {
        await _dbContext.Resources.AddAsync(resource);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Resource?> GetById(Guid resourceId)
    {
        return await _dbContext.Resources.FirstOrDefaultAsync(x => x.Id == resourceId);
    }

    public async Task<List<Resource>> GetByType(string resourceType)
    {
        return await _dbContext.Resources
            .Where(x => x.Type == resourceType)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<bool> Update(Resource resource)
    {
        _dbContext.Resources.Update(resource);
        
        var result = await _dbContext.SaveChangesAsync();
        if (result < 0)
        {
            throw new DbUpdateException("Update operation failed!");
        }
        
        return result > 0;
    }
    
    public async Task<bool> Delete(Resource resource)
    {
        _dbContext.Resources.Remove(resource);

        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}