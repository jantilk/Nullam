using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPersonRepository
{
    Task Add(Person person);
    Task<List<Person>> Get(FilterDto? filter);
    Task<Person?> GetById(Guid personId);
    Task<bool> Update(Person updatedPerson);
}