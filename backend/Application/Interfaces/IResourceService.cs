using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IResourceService
{
    Task<OperationResult<List<GetResourcesByTypeResponse>>?> GetByType(string type);
    Task<OperationResult<bool>> Add(AddResourceRequest request);
    Task<OperationResult<bool>> Update(Guid id, UpdateResourceRequest request);
    Task<OperationResult<bool>> Delete(Guid id);
}