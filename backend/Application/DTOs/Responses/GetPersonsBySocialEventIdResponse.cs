using Domain.Entities;

namespace Application.DTOs.Responses;

public class GetPersonsBySocialEventIdResponse
{
    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string IdCode { get; set; }
    public required Resource PaymentType { get; set; }
    public string? AdditionalInfo { get; set; }
}