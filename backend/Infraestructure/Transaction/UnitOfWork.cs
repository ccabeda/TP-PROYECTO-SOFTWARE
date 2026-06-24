using Microsoft.EntityFrameworkCore.Storage;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Transaction
{
    public class UnitOfWork : IApplicationUnitOfWork
    {
        private readonly AplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction is not null)
            {
                return;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                return;
            }

            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                return;
            }

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
