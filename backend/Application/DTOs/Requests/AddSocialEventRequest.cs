using System.ComponentModel.DataAnnotations;
using Application.ValidationAttributes;

namespace Application.DTOs.Requests;

public class AddSocialEventRequest
{
    [Required]
    [MaxLength(75)]
    public required string Name { get; set; }

    [Required]
    [DateInTheFuture]
    public required DateTime Date { get; set; }

    [Required]
    [MaxLength(75)]
    public required string Location { get; set; }

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }
}