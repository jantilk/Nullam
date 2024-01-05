using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class SocialEventCompany : ParticipantBase
{
    public required Guid SocialEventId { get; set; }
    public required Guid CompanyId { get; set; }
    public required int NumberOfParticipants { get; set; }
    
    public SocialEvent? SocialEvent { get; set; }
    public Company? Company { get; set; }
}