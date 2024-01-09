using Domain.Entities;

namespace Application.Interfaces;

public interface IPersonRepository
{
    Task Add(Person person);
    Task<Person?> Get(Guid personId);
    Task<bool> Update(Person updatedPerson);
}