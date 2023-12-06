using iBudget.DAO;
using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace iBudget.Repository
{
    public class LoginRepository : IRepository<LoginModel>
    {
        private readonly ApplicationDBContext _context;

        public LoginRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LoginModel> GetByUsernameAsync(string username)
        {
            return await _context.Login.FirstOrDefaultAsync(l => l.Username.Equals(username));
        }

        public async Task SaveLoginLog(LoginLogModel loginLog)
        {
            _ = await _context.LoginLog.AddAsync(loginLog);
            _ = await _context.SaveChangesAsync();
        }

        public async Task AddAsync(LoginModel login)
        {
            _ = await _context.Login.AddAsync(login);
            _ = await _context.SaveChangesAsync();
        }

        public Task<LoginModel> GetByIdAsync(int id, Enum[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LoginModel>> GetAllAsync(Enum[] includes)
        {
            return await _context.Login
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<IEnumerable<LoginModel>> FindAsync(Expression<Func<LoginModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LoginModel entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(LoginModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
