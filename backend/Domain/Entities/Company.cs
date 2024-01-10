using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class Company : EntityBase
{
    [MaxLength(100)]
    public required string Name { get; set; }
    [MinLength(8)]
    [MaxLength(8)]
    public required int RegisterCode { get; set; }
}