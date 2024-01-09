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

    public async Task<Person?> Get(Guid personId)
    {
        return await _dbContext.Persons.FirstOrDefaultAsync(x => x.Id == personId);
    }

    public async Task<bool> Update(Person updatedPerson)
    {
        _dbContext.Persons.Update(updatedPerson);
        
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