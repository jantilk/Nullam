using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class SocialEventPerson : ParticipantBase
{
    public required Guid SocialEventId { get; set; }
    public required Guid PersonId { get; set; }
    
    public SocialEvent? SocialEvent { get; set; }
    public Person? Person { get; set; }
}