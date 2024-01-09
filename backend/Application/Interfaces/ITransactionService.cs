namespace Application.Interfaces;

public interface ITransactionService
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}