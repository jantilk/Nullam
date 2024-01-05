using Domain.Entities.Abstracts;

namespace Domain.Entities;

public class Person : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string IdCode { get; set; }
}