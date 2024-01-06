using Application.Common;
using Application.DTOs;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventsService
{
    Task<OperationResult<List<GetSocialEventsResponse>>?> Get(SortingOption? orderBy, FilterDto? filter);
}