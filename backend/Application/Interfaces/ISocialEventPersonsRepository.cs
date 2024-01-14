using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Interfaces;

public interface ISocialEventPersonsRepository
{
    Task Add(Guid socialEventId, Guid personId, AddSocialEventPersonRequest request);
    Task<List<SocialEventPerson>> GetSocialEventPersonsBySocialEventId(Guid socialEventId);
    Task<SocialEventPerson?> GetSocialEventsByPersonId(Guid socialEventId, Guid personId);
    Task<SocialEventPerson?> GetSocialEventPerson(Guid socialEventId, Guid personId);
    Task<bool> Update(SocialEventPerson updatedSocialEventPerson);
    Task<bool> Delete(SocialEventPerson socialEventPerson);
    Task<List<SocialEventPerson>> GetByResourceId(Guid resourceId);
}