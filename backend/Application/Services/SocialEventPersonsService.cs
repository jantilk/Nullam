using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class SocialEventPersonsService : ISocialEventPersonsService
{
    private readonly ITransactionService _transactionService;
    private readonly IPersonRepository _personRepository;
    private readonly ISocialEventPersonsRepository _socialEventPersonsRepository;

    public SocialEventPersonsService(ITransactionService transactionService, IPersonRepository personRepository, ISocialEventPersonsRepository socialEventPersonsRepository)
    {
        _transactionService = transactionService;
        _personRepository = personRepository;
        _socialEventPersonsRepository = socialEventPersonsRepository;
    }
    
    public async Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventPersonRequest request)
    {
        try
        {
            await _transactionService.BeginTransactionAsync();

            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                CreatedAt = DateTime.Now,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdCode = request.IdCode
            };
            
            await _personRepository.Add(person);
            await _socialEventPersonsRepository.Add(socialEventId, personId, request);
            
            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true); 
        }
        catch (Exception ex)
        {
            // TODO: how to check here if current transaction is null
            await _transactionService.RollbackTransactionAsync();

            return OperationResult<bool>.Failure($"Failed to add social event person! {ex}");
        }
    }

    public async Task<OperationResult<List<GetPersonsBySocialEventIdResponse>>?> GetBySocialEventId(Guid socialEventId)
    {
        try
        {
            var socialEventPersons = await _socialEventPersonsRepository.GetBySocialEventId(socialEventId);
            
            if (socialEventPersons.Count == 0)
            {
                return null;
            }

            var response = socialEventPersons
                .Select(x => new GetPersonsBySocialEventIdResponse
                {
                    Id = x.PersonId,
                    CreatedAt = x.CreatedAt,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                    IdCode = x.Person.IdCode,
                    PaymentType = x.PaymentType,
                    AdditionalInfo = x.AdditionalInfo
                })
                .ToList();
            
            return OperationResult<List<GetPersonsBySocialEventIdResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<GetPersonsBySocialEventIdResponse>>.FailureWithLog($"Failed to get social event persons! {ex.Message}");
        }
    }

    public async Task<OperationResult<GetSocialEventPersonResponse>?> GetByPersonId(Guid socialEventId, Guid personId)
    {
        try
        {
            var socialEventPerson = await _socialEventPersonsRepository.GetByPersonId(socialEventId, personId);
            if (socialEventPerson == null)
            {
                return null;
            }

            var response = new GetSocialEventPersonResponse
            {
                SocialEventId = socialEventPerson.SocialEventId,
                CompanyId = socialEventPerson.PersonId,
                CreatedAt = socialEventPerson.CreatedAt,
                PaymentType = socialEventPerson.PaymentType,
                AdditionalInfo = socialEventPerson.AdditionalInfo,
                Person = new PersonResponse
                {
                    Id = socialEventPerson.Person.Id,
                    CreatedAt = socialEventPerson.Person.CreatedAt,
                    FirstName = socialEventPerson.Person.FirstName,
                    LastName = socialEventPerson.Person.LastName,
                    IdCode = socialEventPerson.Person.IdCode
                },
            };
            
            return OperationResult<GetSocialEventPersonResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<GetSocialEventPersonResponse>.Failure($"Failed to add social event person! {ex}");
        }
    }

    public async Task<OperationResult<bool>?> Update(Guid socialEventId, Guid personId, UpdateSocialEventPersonRequest request)
    {
        try
        {
            await _transactionService.BeginTransactionAsync();

            var socialEventPerson = await _socialEventPersonsRepository.GetSocialEventPerson(socialEventId, personId);
            if (socialEventPerson == null) {
                return OperationResult<bool>.Failure("Social event person not found");
            }
            
            var person = await _personRepository.Get(personId);
            if (person == null) {
                return OperationResult<bool>.Failure("Person not found");
            }

            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.IdCode = request.IdCode;
            
            await _personRepository.Update(person);

            socialEventPerson.PaymentType = request.PaymentType;
            socialEventPerson.AdditionalInfo = request.AdditionalInfo;
            
            await _socialEventPersonsRepository.Update(socialEventPerson);

            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _transactionService.RollbackTransactionAsync();
            
            return OperationResult<bool>.Failure($"Failed to update social event company! {ex}");
        }
    }

    public async Task<OperationResult<bool>> Delete(Guid socialEventId, Guid personId)
    {
        try
        {
            var socialEventPerson = await _socialEventPersonsRepository.GetByPersonId(socialEventId, personId);

            if (socialEventPerson == null)
            {
                return OperationResult<bool>.Failure("Failed to delete, social event person does not exist!");
            }
            
            var result = await _socialEventPersonsRepository.Delete(socialEventPerson);

            if (!result) {
                return OperationResult<bool>.Failure("Failed to Delete social event person");
            }
            
            return OperationResult<bool>.Success(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}