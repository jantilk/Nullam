namespace Application.DTOs.Responses;

public class CompanyResponse
{
    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string Name { get; set; }
    public required string RegisterCode { get; set; }
}