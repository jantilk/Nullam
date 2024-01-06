using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ISocialEventsRepository
{
    Task<List<SocialEvent>> Get(SortingOption? orderBy, FilterDto? filter);
    Task<SocialEvent?> GetById(Guid id);
    Task<SocialEvent> Add(SocialEvent socialEvent);
    Task<bool> Update(SocialEvent meetup);
    Task<bool> Delete(SocialEvent meetup);
}