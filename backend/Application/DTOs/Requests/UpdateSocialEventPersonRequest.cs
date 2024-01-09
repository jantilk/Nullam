using Domain.Enums;

namespace Application.DTOs.Requests;

public class UpdateSocialEventPersonRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string IdCode { get; set; }
    public required PaymentType PaymentType { get; set; }
    public required string? AdditionalInfo { get; set; }
}