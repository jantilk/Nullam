using Domain.Enums;

namespace Application.DTOs.Requests;

public class AddSocialEventCompanyRequest
{
    public required string Name { get; set; }
    public required string RegisterCode { get; set; }
    public required PaymentType PaymentType { get; set; }
    public required int NumberOfParticipants { get; set; }
    public string? AdditionalInfo { get; set; }
}