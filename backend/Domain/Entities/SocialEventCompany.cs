using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class SocialEventCompany : ParticipantBase
{
    public required Guid SocialEventId { get; set; }
    public required Guid CompanyId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The number of participants must be greater than 0.")]
    public required int NumberOfParticipants { get; set; }
    
    //TODO: Maybe 4000 is ok also? Everything over 4000 will make it nvarchar(max). Can be increased easily later.
    [MaxLength(4000)]
    public string? AdditionalInfo { get; set; }
    
    public SocialEvent? SocialEvent { get; set; }
    public Company? Company { get; set; }
}