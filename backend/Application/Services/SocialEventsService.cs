using System.Diagnostics;
using System.Globalization;
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
    
    public async Task<OperationResult<bool>> Add(AddSocialEventRequest request)
    {
        Console.WriteLine(request.Date.Kind);
        Console.WriteLine(request.Date.ToString(CultureInfo.InvariantCulture));
        
        try
        {
            var socialEvent = new SocialEvent
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = request.Name,
                Date = request.Date,
                Location = request.Location,
                AdditionalInfo = request.AdditionalInfo,
            };

            await _socialEventsRepository.Add(socialEvent);

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"Get operation failed. {ex.Message}");
        }   
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
            return OperationResult<List<GetSocialEventsResponse>>.FailureWithLog($"Get operation failed. {ex.Message}");
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
            Console.WriteLine(socialEvent.Date.Kind);
            Console.WriteLine(socialEvent.Date.ToString(CultureInfo.InvariantCulture));

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
            return OperationResult<GetSocialEventByIdResponse>.Failure($"Get operation failed. {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> Update(Guid id, UpdateSocialEventRequest request)
    {
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
            return OperationResult<bool>.Failure($"Update operation failed! {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> Delete(Guid id)
    {
        try
        {
            var socialEvent = await _socialEventsRepository.GetById(id);
            
            if (socialEvent == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Social event not found.");
            }

            var result = await _socialEventsRepository.Delete(socialEvent);
            
            if (!result) {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed.");
            }

            return OperationResult<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. {ex.Message}");
        }
    }
}