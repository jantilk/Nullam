namespace Application.DTOs.Responses;

public class GetSocialEventsResponse : ResponseBase
{
    public required string Name { get; set; }
    public required DateTime Date { get; set; }
}