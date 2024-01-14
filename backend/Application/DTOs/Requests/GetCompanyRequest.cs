namespace Application.DTOs.Requests;

public class GetCompanyRequest
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public int? RegisterCode { get; set; }
}