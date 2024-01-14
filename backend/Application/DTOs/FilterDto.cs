using Application.ValidationAttributes;

namespace Application.DTOs;

[DateRange]
public class FilterDto
{
    public string? SearchTerm { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}