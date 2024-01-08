using Application.Common;
using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;

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

    public async Task<OperationResult<GetSocialEventByIdResponse>?> GetById(Guid id)
    {
        try
        {
            var socialEvent = await _socialEventsRepository.GetById(id);

            if (socialEvent == null)
            {
                return null;
            }

            var response = new GetSocialEventByIdResponse()
            {
                Id = socialEvent.Id,
                Name = socialEvent.Name,
                Date = socialEvent.Date,
                Location = socialEvent.Location
            };
            
            return OperationResult<GetSocialEventByIdResponse>.Success(response);
        }
        catch (Exception ex)
        {
            // TODO: maybe dont return ex.message here?
            return OperationResult<GetSocialEventByIdResponse>.Failure($"Failed to get social event! {ex.Message}");
        }
    }

    public async Task<OperationResult<AddSocialEventResponse>?> Add(AddSocialEventRequest request)
    {
        try
        {
            var socialEvent = new SocialEvent
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Name = request.Name,
                Date = DateTime.Parse(request.Date),
                Location = request.Location,
                AdditionalInfo = request.AdditionalInfo,
            };

            var result = await _socialEventsRepository.Add(socialEvent);

            var response = new AddSocialEventResponse()
            {
                Id = result.Id,
                Date = result.Date,
                Location = result.Location,
                AdditionalInfo = result.AdditionalInfo
            };
            
            return OperationResult<AddSocialEventResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<AddSocialEventResponse>.Failure($"Failed add social event! {ex.Message}");
        }   
    }

    public async Task<OperationResult<bool>> Update(Guid id, UpdateSocialEventRequest request)
    {
        // TODO: check all over the code that no meetup exists
        try
        {
            var socialEvent = await _socialEventsRepository.GetById(id);
            
            if (socialEvent == null)
            {
                return OperationResult<bool>.Failure("Update operation failed, social event not found.");
            }

            socialEvent.Name = request.Name;
            socialEvent.Date = request.Date;
            socialEvent.Location = request.Location;
            socialEvent.AdditionalInfo = request.AdditionalInfo;
            
            await _socialEventsRepository.Update(socialEvent);

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // TODO: maybe change texts to "Update operation failed"
            return OperationResult<bool>.Failure($"Failed to update social event! {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> Delete(Guid id)
    {
        try
        {
            var socialEvent = await _socialEventsRepository.GetById(id);
            
            if (socialEvent == null)
            {
                return OperationResult<bool>.Failure("Failed to delete, social event does not exist!");
            }

            var result = await _socialEventsRepository.Delete(socialEvent);
            
            if (!result) {
                return OperationResult<bool>.Failure("Failed to Delete social event");
            }

            return OperationResult<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"Failed to delete social event! {ex.Message}");
        }
    }
}