using Application.ValidationAttributes;

namespace Application.DTOs;

[DateRange]
public class FilterDto
{
    public string? Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public bool IsValid()
    {
        return EndDate >= StartDate;
    }
}