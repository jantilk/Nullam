using System.ComponentModel.DataAnnotations;
using Application.ValidationAttributes;
using Domain.Entities;
using Domain.Enums;
using Domain.ValidationAttributes;

namespace Application.DTOs.Requests;

public class UpdateSocialEventPersonRequest
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
    public required Guid PaymentTypeId { get; set; }
    [MaxLength(1500)]
    public string? AdditionalInfo { get; set; }
}