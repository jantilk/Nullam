using System.ComponentModel.DataAnnotations;
using Application.ValidationAttributes;
using Domain.ValidationAttributes;

namespace Application.DTOs.Requests;

public class UpdateSocialEventRequest
{
    [Required]
    [MaxLength(250)]
    public required string Name { get; set; }

    [Required]
    [FutureDate]
    public required DateTime Date { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Location { get; set; }

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }
}