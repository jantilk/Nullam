using Application.DTOs;
using Application.DTOs.Responses;
using Application.Interfaces;

namespace Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public async Task<OperationResult<List<PersonResponse>>> Get(FilterDto? filter)
    {
        try
        {
            var persons = await _personRepository.Get(filter);
            
            var response = persons
                .Select(x => new PersonResponse
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CreatedAt = x.CreatedAt,
                    IdCode = x.IdCode
                })
                .ToList();
            
            return OperationResult<List<PersonResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<PersonResponse>>.FailureWithLog($"{nameof(Get)} operation failed. {ex.Message}");
        }
    }
}