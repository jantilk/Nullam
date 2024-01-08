namespace Application.DTOs.Responses;

public class GetSocialEventByIdResponse : ResponseBase
{
    public required string Name { get; set;}
    public required DateTime Date { get; set;}
    public required string Location { get; set;}
}