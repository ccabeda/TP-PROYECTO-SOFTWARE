namespace TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork
{
    public interface IApplicationUnitOfWork
    {
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
