namespace Application.DTOs.Responses;

public class AddSocialEventResponse : ResponseBase
{
    public required DateTime Date { get; set; }
    public required string Place { get; set; }
    public string? AdditionalInfo { get; set; }
}