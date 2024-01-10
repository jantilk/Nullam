using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class SocialEventPerson : ParticipantBase
{
    public required Guid SocialEventId { get; set; }
    public required Guid PersonId { get; set; }
    [MaxLength(1500)]
    public string? AdditionalInfo { get; set; }

    public SocialEvent? SocialEvent { get; set; }
    public Person? Person { get; set; }
}