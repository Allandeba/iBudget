using getQuote.Framework;
using getQuote.Models;
using getQuote.Repository;

namespace getQuote.Business
{
    public class LoginBusiness
    {
        private readonly LoginRepository _repository;

        public LoginBusiness(LoginRepository loginRepository)
        {
            _repository = loginRepository;
        }

        public async Task Login(LoginModel login)
        {
            LoginModel user = await _repository.GetByUsernameAsync(login.Username);
            if (user == null)
            {
                throw new Exception("User not valid!");
            }

            Cryptography Cryptography = new();
            if (!user.Password.Equals(Cryptography.GetHash(login.Password)))
            {
                throw new Exception("Password not valid!");
            }
        }

        public async Task SaveLoginLog(LoginLogModel loginLog)
        {
            Cryptography cryptography = new();
            loginLog.Password = cryptography.GetHash(loginLog.Password);
            await _repository.SaveLoginLog(loginLog);
        }
    }
}
