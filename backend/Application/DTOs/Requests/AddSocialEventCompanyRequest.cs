using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Requests;

public class AddSocialEventCompanyRequest
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    [Required]
    [Range(10000000, 99999999)]
    public required int RegisterCode { get; set; }
    [Required]
    public required Guid PaymentTypeId { get; set; }
    [Required]
    public required int NumberOfParticipants { get; set; }
    [MaxLength(5000)]
    public string? AdditionalInfo { get; set; }
}