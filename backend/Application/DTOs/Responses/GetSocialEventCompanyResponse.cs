using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Responses;

public class GetSocialEventCompanyResponse
{
    public required Guid SocialEventId { get; set; }
    public required Guid CompanyId { get; set; }
    public required int NumberOfParticipants { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Resource PaymentType { get; set; }
    public required string? AdditionalInfo { get; set; }
    public required CompanyResponse Company { get; set; }
}