using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class Resource : EntityBase
{
    [MaxLength(500)]
    public required string Type { get; set; }
    [MaxLength(500)]
    public required string Text { get; set; }
}