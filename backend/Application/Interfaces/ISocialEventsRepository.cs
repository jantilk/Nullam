using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ISocialEventsRepository
{
    Task<List<SocialEvent>> Get(SortingOption? orderBy, FilterDto? filter);
}