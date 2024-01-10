using Application.Common;
using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventsService
{
    Task<OperationResult<bool>> Add(AddSocialEventRequest request);
    Task<OperationResult<List<GetSocialEventsResponse>>?> Get(SortingOption? orderBy, FilterDto? filter);
    Task<OperationResult<GetSocialEventByIdResponse>?> GetById(Guid id);
    Task<OperationResult<bool>> Update(Guid id, UpdateSocialEventRequest request);
    Task<OperationResult<bool>> Delete(Guid id);
}