using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventPersonsService
{
    Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventPersonRequest request);
    Task<OperationResult<List<GetPersonsBySocialEventIdResponse>>> GetBySocialEventId(Guid socialEventId);
    Task<OperationResult<GetSocialEventPersonResponse>?> GetByPersonId(Guid socialEventId, Guid personId);
    Task<OperationResult<bool>?> Update(Guid socialEventId, Guid personId, UpdateSocialEventPersonRequest request);
    Task<OperationResult<bool>> Delete(Guid socialEventId, Guid personId);
}