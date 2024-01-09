namespace Application.DTOs.Responses;

public class GetCompaniesBySocialEventIdResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string RegisterCode { get; set; }
}