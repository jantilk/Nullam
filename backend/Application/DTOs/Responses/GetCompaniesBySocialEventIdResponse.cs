namespace Application.DTOs.Responses;

public class GetCompaniesBySocialEventIdResponse
{
    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string Name { get; set; }
    public required int RegisterCode { get; set; }
}