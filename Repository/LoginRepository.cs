using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Repository
{
    public class LoginRepository
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
    }
}
