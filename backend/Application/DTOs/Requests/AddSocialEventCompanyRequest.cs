using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Requests;

public class AddSocialEventCompanyRequest
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(8)]
    public required int RegisterCode { get; set; }
    [Required]
    public required PaymentType PaymentType { get; set; }
    [Required]
    public required int NumberOfParticipants { get; set; }
    [MaxLength(5000)]
    public string? AdditionalInfo { get; set; }
}