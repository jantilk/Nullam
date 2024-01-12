using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstracts;
using Domain.ValidationAttributes;

namespace Domain.Entities;

public class Person : EntityBase
{
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [MaxLength(50)]
    public required string LastName { get; set; }
    [EstonianIdCode]
    [MaxLength(11)]
    public required string IdCode { get; set; }
}