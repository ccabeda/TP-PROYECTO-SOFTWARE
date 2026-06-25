namespace TP_PROYECTO_SOFTWARE.Application.IUnitOfWork
{
    public interface IApplicationUnitOfWork
    {
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
