using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly NullamDbContext _dbContext;

    public PersonRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Person person)
    {
        await _dbContext.Persons.AddAsync(person);
    }

    public async Task<List<Person>> Get(FilterDto? filter)
    {
        var query = _dbContext.Persons.AsQueryable();
        
        if (filter != null && !string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(p => p.FirstName.ToLower().Contains(searchTermLower)
                                     || p.LastName.ToLower().Contains(searchTermLower)
                                     || p.IdCode.ToLower().Contains(searchTermLower));
        }
        
        var result = await query.ToListAsync();

        return result;
    }
    
    public async Task<Person?> GetById(Guid personId)
    {
        return await _dbContext.Persons.FirstOrDefaultAsync(x => x.Id == personId);
    }

    public async Task<bool> Update(Person updatedPerson)
    {
        _dbContext.Persons.Update(updatedPerson);
        
        var result = await _dbContext.SaveChangesAsync();
        if (result < 0)
        {
            throw new DbUpdateException("Update operation failed!");
        }
        
        return result > 0;
    }
}