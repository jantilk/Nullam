using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class SocialEventPersonsService : ISocialEventPersonsService
{
    private readonly ITransactionService _transactionService;
    private readonly IPersonRepository _personRepository;
    private readonly ISocialEventPersonsRepository _socialEventPersonsRepository;
    private readonly IResourceRepository _resourceRepository;

    public SocialEventPersonsService(
        ITransactionService transactionService,
        IPersonRepository personRepository,
        ISocialEventPersonsRepository socialEventPersonsRepository,
        IResourceRepository resourceRepository)
    {
        _transactionService = transactionService;
        _personRepository = personRepository;
        _socialEventPersonsRepository = socialEventPersonsRepository;
        _resourceRepository = resourceRepository;
    }
    
    public async Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventPersonRequest request)
    {
        try
        {
            await _transactionService.BeginTransactionAsync();

            var getPersonRequest = new GetPersonRequest
            {
                IdCode = request.IdCode
            };
            
            var person = await _personRepository.Get(getPersonRequest);
            if (person == null)
            {
                person = new Person
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IdCode = request.IdCode
                };
                
                await _personRepository.Add(person);
            }

            var socialEventPerson = await _socialEventPersonsRepository.GetSocialEventsByPersonId(socialEventId, person.Id);
            if (socialEventPerson != null) {
                return OperationResult<bool>.Failure($"Person with id code {person.IdCode} is already registered to this event.", StatusCodes.Status409Conflict);
            }
            
            await _socialEventPersonsRepository.Add(socialEventId, person.Id, request);
            
            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true); 
        }
        catch (Exception ex)
        {
            await _transactionService.RollbackTransactionAsync();

            // TODO: dont return such exception to frontend.
            // TODO: check everywhere in code.
            return OperationResult<bool>.Failure($"{nameof(Add)} operation failed. {ex}");
        }
    }

    public async Task<OperationResult<List<GetPersonsBySocialEventIdResponse>>> GetSocialEventPersonsBySocialEventId(Guid socialEventId)
    {
        try
        {
            var socialEventPersons = await _socialEventPersonsRepository.GetSocialEventPersonsBySocialEventId(socialEventId);
            
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
            return OperationResult<List<GetPersonsBySocialEventIdResponse>>.Failure($"{nameof(GetSocialEventPersonsBySocialEventId)} operation failed. {ex}");
        }
    }

    public async Task<OperationResult<GetSocialEventsByPersonIdResponse>?> GetSocialEventsByPersonId(Guid socialEventId, Guid personId)
    {
        try
        {
            var socialEventPerson = await _socialEventPersonsRepository.GetSocialEventsByPersonId(socialEventId, personId);
            if (socialEventPerson == null)
            {
                return null;
            }

            var response = new GetSocialEventsByPersonIdResponse
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
            
            return OperationResult<GetSocialEventsByPersonIdResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<GetSocialEventsByPersonIdResponse>.Failure($"{nameof(GetSocialEventsByPersonId)} operation failed. {ex}");
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
            
            var person = await _personRepository.GetById(personId);
            if (person == null) {
                return OperationResult<bool>.Failure("Person not found");
            }

            var paymentType = await _resourceRepository.GetById(request.PaymentTypeId);
            if (paymentType == null) {
                return OperationResult<bool>.Failure("PaymentType not found");
            }
            
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.IdCode = request.IdCode;
            
            await _personRepository.Update(person);

            socialEventPerson.PaymentType = paymentType;
            socialEventPerson.AdditionalInfo = request.AdditionalInfo;
            
            await _socialEventPersonsRepository.Update(socialEventPerson);

            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _transactionService.RollbackTransactionAsync();
            
            return OperationResult<bool>.Failure($"{nameof(Update)} operation failed. {ex}");
        }
    }

    public async Task<OperationResult<bool>> Delete(Guid socialEventId, Guid personId)
    {
        try
        {
            var socialEventPerson = await _socialEventPersonsRepository.GetSocialEventsByPersonId(socialEventId, personId);

            if (socialEventPerson == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Social event person not found");
            }
            
            var result = await _socialEventPersonsRepository.Delete(socialEventPerson);

            if (!result) {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed.");
            }
            
            return OperationResult<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. {ex}");
        }
    }
}