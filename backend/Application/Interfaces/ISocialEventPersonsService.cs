using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventPersonsService
{
    Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventPersonRequest request);
    Task<OperationResult<List<GetPersonsBySocialEventIdResponse>>> GetSocialEventPersonsBySocialEventId(Guid socialEventId);
    Task<OperationResult<GetSocialEventsByPersonIdResponse>?> GetSocialEventsByPersonId(Guid socialEventId, Guid personId);
    Task<OperationResult<bool>?> Update(Guid socialEventId, Guid personId, UpdateSocialEventPersonRequest request);
    Task<OperationResult<bool>> Delete(Guid socialEventId, Guid personId);
}