using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ISocialEventsRepository
{
    Task Add(SocialEvent socialEvent);
    Task<List<SocialEvent>> Get(SortingOption? orderBy, FilterDto? filter);
    Task<SocialEvent?> GetById(Guid socialEventId);
    Task<bool> Update(SocialEvent socialEvent);
    Task<bool> Delete(SocialEvent socialEvent);
}