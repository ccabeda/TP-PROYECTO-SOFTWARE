using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command
{
    public class RepositoryUserCommand : IRepositoryUserCommand
    {
        private readonly ApplicationDbContext _context;

        public RepositoryUserCommand(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(User user) => await _context.Users.AddAsync(user);

        public async Task Save() => await _context.SaveChangesAsync();
    }
}

