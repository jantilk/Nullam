using Domain.Enums;

namespace Application.DTOs.Responses;

public class GetSocialEventPersonResponse
{
    public required Guid SocialEventId { get; set; }
    public required Guid CompanyId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required PaymentType PaymentType { get; set; }
    public required string? AdditionalInfo { get; set; }
    public required PersonResponse Person { get; set; }
}