namespace Application.DTOs.Requests;

public class GetPersonRequest
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? IdCode { get; set; }
}