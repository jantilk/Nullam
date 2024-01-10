using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.ValidationAttributes;

namespace Application.DTOs.Requests;

public class AddSocialEventPersonRequest
{
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }
    [Required]
    [EstonianIdCode]
    public required string IdCode { get; set; }
    [Required]
    public required PaymentType PaymentType { get; set; }
    [MaxLength(1500)]
    public string? AdditionalInfo { get; set; }
}