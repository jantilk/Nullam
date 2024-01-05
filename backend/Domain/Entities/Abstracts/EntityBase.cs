namespace Domain.Entities.Abstracts;

public abstract class EntityBase
{
    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
}