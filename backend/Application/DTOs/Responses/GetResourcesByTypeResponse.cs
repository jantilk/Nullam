namespace Application.DTOs.Responses;

public class GetResourcesByTypeResponse : ResponseBase
{
    public required string Type { get; set; }
    public required string Text { get; set; }
}