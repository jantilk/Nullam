using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class SocialEventPersonsService : ISocialEventPersonsService
{
    private readonly ITransactionService _transactionService;
    private readonly IPersonRepository _personRepository;
    private readonly ISocialEventPersonsRepository _socialEventPersonsRepository;
    private readonly IResourceRepository _resourceRepository;

    public SocialEventPersonsService(ITransactionService transactionService, IPersonRepository personRepository, ISocialEventPersonsRepository socialEventPersonsRepository, IResourceRepository resourceRepository)
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
            await _transactionService.RollbackTransactionAsync();

            return OperationResult<bool>.Failure($"{nameof(Add)} operation failed. {ex}");
        }
    }

    public async Task<OperationResult<List<GetPersonsBySocialEventIdResponse>>> GetBySocialEventId(Guid socialEventId)
    {
        try
        {
            var socialEventPersons = await _socialEventPersonsRepository.GetBySocialEventId(socialEventId);
            
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
            return OperationResult<List<GetPersonsBySocialEventIdResponse>>.Failure($"{nameof(GetBySocialEventId)} operation failed. {ex}");
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
            return OperationResult<GetSocialEventPersonResponse>.Failure($"{nameof(GetByPersonId)} operation failed. {ex}");
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
            var socialEventPerson = await _socialEventPersonsRepository.GetByPersonId(socialEventId, personId);

            if (socialEventPerson == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Social event person not found");
            }
            
            if (socialEventPerson.SocialEvent.Date < DateTime.UtcNow) {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Cannot delete person from past event.");
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