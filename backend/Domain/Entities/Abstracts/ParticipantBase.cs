using Domain.Enums;

namespace Domain.Entities.Abstracts;

public abstract class ParticipantBase
{
    public required DateTime CreatedAt { get; set; }
    public required Guid ResourceId { get; set; }
    
    public Resource? PaymentType { get; set; }
}