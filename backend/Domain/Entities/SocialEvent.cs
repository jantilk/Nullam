using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class SocialEvent : EntityBase
{
    public required string Name { get; set; }
    public required DateTime Date { get; set; }
    public required string Location { get; set; }
    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }

    public ICollection<SocialEventCompany> Companies { get; set; } = new List<SocialEventCompany>();
    public ICollection<SocialEventPerson> Persons { get; set; } = new List<SocialEventPerson>();
}