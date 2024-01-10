using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Requests;

public class UpdateSocialEventCompanyRequest
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    [Required]
    [Range(10000000, 99999999)]
    public required int RegisterCode { get; set; }
    [Required]
    public required PaymentType PaymentType { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The number of participants must be greater than 0.")]
    public required int NumberOfParticipants { get; set; }
    [MaxLength(5000)]
    public string? AdditionalInfo { get; set; }
}