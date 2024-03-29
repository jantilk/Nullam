using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Responses;

public class GetSocialEventsByPersonIdResponse
{
    public required Guid SocialEventId { get; set; }
    public required Guid CompanyId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Resource PaymentType { get; set; }
    public required string? AdditionalInfo { get; set; }
    public required PersonResponse Person { get; set; }
}