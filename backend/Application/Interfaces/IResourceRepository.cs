using Domain.Entities;

namespace Application.Interfaces;

public interface IResourceRepository
{
    Task Add(Resource resource);
    Task<Resource?> GetById(Guid resourceId);
    Task<List<Resource>> GetByType(string resourceType);
    Task<bool> Update(Resource resource);
    Task<bool> Delete(Resource resource);
}