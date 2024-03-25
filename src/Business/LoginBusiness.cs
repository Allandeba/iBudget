using iBudget.DAO.Entities;
using iBudget.Framework;
using iBudget.Repository;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace iBudget.Business;

public class LoginBusiness
{
    private readonly LoginRepository _repository;

    public LoginBusiness(LoginRepository loginRepository)
    {
        _repository = loginRepository;
    }

    public async Task Login(LoginModel login)
    {
        var user = await _repository.GetByUsernameAsync(login.Username);
        if (user == null)
            throw new Exception(Messages.InvalidUsernameOrPassword);

        Cryptography Cryptography = new();
        if (!user.Password.Equals(Cryptography.GetHash(login.Password)))
            throw new Exception(Messages.InvalidUsernameOrPassword);
    }

    public async Task SaveLoginLog(LoginLogModel loginLog)
    {
        Cryptography cryptography = new();
        loginLog.Password = cryptography.GetHash(loginLog.Password);
        await _repository.SaveLoginLog(loginLog);
    }

    public async Task AddAsync(LoginModel login)
    {
        IEnumerable<LoginModel> loginModelList = await _repository
            .GetAll(Array.Empty<Enum>())
            .ToListAsync();
        if (loginModelList.Any())
            throw new Exception("Não é permitido adicionar um novo usuário");

        Cryptography cryptography = new();
        login.Password = cryptography.GetHash(login.Password);
        await _repository.AddAsync(login);
    }
}