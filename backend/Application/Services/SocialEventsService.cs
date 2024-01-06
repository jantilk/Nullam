using Application.Common;
using Application.DTOs;
using Application.DTOs.Responses;
using Application.Interfaces;

namespace Application.Services;

public class SocialEventsService : ISocialEventsService
{
    private readonly ISocialEventsRepository _socialEventsRepository;

    public SocialEventsService(ISocialEventsRepository socialEventsRepository)
    {
        _socialEventsRepository = socialEventsRepository;
    }
    
    public async Task<OperationResult<List<GetSocialEventsResponse>>?> Get(SortingOption? orderBy, FilterDto? filter)
    {
        try
        {
            var socialEvents = await _socialEventsRepository.Get(orderBy, filter);
            
            var response = socialEvents
                .Select(x => new GetSocialEventsResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Date = x.Date
                })
                .ToList();
            
            return OperationResult<List<GetSocialEventsResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<GetSocialEventsResponse>>.FailureWithLog($"Failed to get social events! {ex.Message}");
        }
    }
}