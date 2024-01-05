using Domain.Enums;

namespace Domain.Entities.Abstracts;

public abstract class ParticipantBase
{
    public required DateTime CreatedAt { get; set; }
    public required PaymentType PaymentType { get; set; }
    public string? AdditionalInfo { get; set; }
}