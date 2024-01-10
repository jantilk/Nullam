using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class Company : EntityBase
{
    [MaxLength(100)]
    public required string Name { get; set; }
    [Range(10000000, 99999999)]
    public required int RegisterCode { get; set; }
}