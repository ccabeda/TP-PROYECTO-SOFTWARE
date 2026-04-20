using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositoryUserCommand : IRepositoryUserCommand
    {
        private readonly AplicationDbContext _context;

        public RepositoryUserCommand(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(USER user) => await _context.USER.AddAsync(user);

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
