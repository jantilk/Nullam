using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public class UpdateResourceRequest
{
    [MaxLength(500)]
    public required string Type { get; set; }
    [MaxLength(500)]
    public required string Text { get; set; }
}