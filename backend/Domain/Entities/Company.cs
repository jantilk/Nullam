using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class Company : EntityBase
{
    public required string Name { get; set; }
    public required string RegisterCode { get; set; }
}