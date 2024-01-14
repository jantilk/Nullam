namespace Application.DTOs.Requests;

public class GetSocialEventsRequest
{
    public SortingOption? OrderBy { get; set; }
    // public FilterDto? Filter { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}