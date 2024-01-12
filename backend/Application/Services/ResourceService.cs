using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ResourceService : IResourceService
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ISocialEventPersonsRepository _socialEventPersonsRepository;
    private readonly ISocialEventCompaniesRepository _socialEventCompaniesRepository;

    public ResourceService(IResourceRepository resourceRepository, ISocialEventPersonsRepository socialEventPersonsRepository, ISocialEventCompaniesRepository socialEventCompaniesRepository)
    {
        _resourceRepository = resourceRepository;
        _socialEventPersonsRepository = socialEventPersonsRepository;
        _socialEventCompaniesRepository = socialEventCompaniesRepository;
    }
    
    public async Task<OperationResult<bool>> Add(AddResourceRequest request)
    {
        try
        {
            var resource = new Resource
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Type = request.Type,
                Text = request.Text,
            };

            await _resourceRepository.Add(resource);

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"Get operation failed. {ex.Message}");
        }  
    }
    
    public async Task<OperationResult<List<GetResourcesByTypeResponse>>?> GetByType(string type)
    {
        try
        {
            var resources = await _resourceRepository.GetByType(type);
            
            var response = resources
                .Select(x => new GetResourcesByTypeResponse
                {
                    Id = x.Id,
                    Type = x.Type,
                    Text = x.Text
                })
                .ToList();
            
            return OperationResult<List<GetResourcesByTypeResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            //TODO: remove all log stuff if not able to implement
            return OperationResult<List<GetResourcesByTypeResponse>>.FailureWithLog($"Get operation failed. {ex.Message}");
        }
    }
    
    public async Task<OperationResult<bool>> Update(Guid id, UpdateResourceRequest request)
    {
        try
        {
            var resource = await _resourceRepository.GetById(id);
            
            if (resource == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Update)} operation failed.", StatusCodes.Status404NotFound);
            }
            
            resource.Type = request.Type;
            resource.Text = request.Text;
            
            await _resourceRepository.Update(resource);

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
            var resource = await _resourceRepository.GetById(id);
            
            if (resource == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed.", StatusCodes.Status404NotFound);
            }
            
            if (await IsResourceInUse(id))
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed.", StatusCodes.Status409Conflict);
            }
            
            var result = await _resourceRepository.Delete(resource);
            
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
    
    private async Task<bool> IsResourceInUse(Guid resourceId)
    {
        var socialEventPersons = await _socialEventPersonsRepository.GetByResourceId(resourceId);
        var socialEventCompanies = await _socialEventCompaniesRepository.GetByResourceId(resourceId);

        return socialEventPersons.Any() || socialEventCompanies.Any();
    }
}